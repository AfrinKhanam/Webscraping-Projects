var chatInputSelector = "input[data-id='webchat-sendbox-input']";
window.directLine = null;
window.current_Context = "undefined";
window.botUserId = null;
// declare default language once chatbot gets loaded. 1 for english, 2 for hindi
window.selectedBotLanguage = 1;
//google transliteration control
window.googleTransliterationControl = null;

function changeLanguage() {
    var languageDiv = $("#languages").length;
    if (languageDiv == 0) {
        $('#webchat div.main').prepend('<div id="languages"> ' +
            '<div class="custom-control custom-radio">' +
            '<input class="custom-control-input" type="radio" id="' + languages[0].languageName + '" value="' + languages[0].languageId + '" name = "customRadio" checked onClick="onLanguageSelect(1)">' +
            '<label class="custom-control-label" for="' + languages[0].languageName + '">&nbsp; &nbsp; English</label>' +
            '</div>' +
            '<div class="custom-control custom-radio">' +
            '<input class="custom-control-input" type="radio" id="' + languages[1].languageName + '" value="' + languages[1].languageId + '" name = "customRadio" onClick="onLanguageSelect(2)">' +
            '<label class="custom-control-label" for="' + languages[1].languageName + '">&nbsp; &nbsp; हिंदी</label>' +
            '</div>' +
            '</div>' +
            '</div>'
        );
    } else {
        $("#languages").toggle('fast');
    }
}

function onLanguageSelect(lang) {
    window.selectedBotLanguage = lang;
    console.log("selected language is : ", window.selectedBotLanguage);
    $('#languages').hide('fast');

    if (window.googleTransliterationControl == null || window.selectedBotLanguage == 2) {
        languageWarning();
        googleTransliterate();
        if ($('#languageWarning').length == 1) {
            $('#webchat').hide();
            $('#languageWarning').show('fast');
        }
    }
    if (window.selectedBotLanguage == 1)
        window.googleTransliterationControl.disableTransliteration();
}
//get languages from api
function getLanguages() {
    var request = $.ajax({
        url: "/Synonyms/GetAllLanguages",
        type: "GET",
        success: function (data) {
            languages = data;
        }
    });
}

function onLoad() {
    var options = {
        sourceLanguage: google.elements.transliteration.LanguageCode.ENGLISH,
        destinationLanguage: [google.elements.transliteration.LanguageCode.HINDI],
        transliterationEnabled: true
    };

    // Create an instance on TransliterationControl with the required
    // options.
    var control = new google.elements.transliteration.TransliterationControl(options);
    window.googleTransliterationControl = control;
    // Enable transliteration in the textbox with id
    var input = $(chatInputSelector);
   
    control.enableTransliteration();
    control.makeTransliteratable([input[0]]);
}


googleTransliterate = function googleTransliterate() {
    google.load("elements", "1", {
        packages: "transliteration"
    });
    onLoad();
}

languageWarning = function languageWarning() {
    var warningDiv = $('#languageWarning').length;
    if (warningDiv == 0) {
        $('#webchat').css('opacity', '0.2').hide('fast');
        $('body').append('<div id="languageWarning" style="color:black";>' +
            '<strong style="color:red; font-size:15px; padding-bottom: 5px;" title="Disclaimer!" > अस्वीकरण !</strong> '+
            '<p>पाठ आपकी भाषा में अनुवादित किया जाएगा और हो सक्ता है कि पुरी तराह सटीक ना हो। अधिक स्पष्टता के लिए, कृपया हमारे फोनबैंकिंग प्रतिनिधि से संपर्क करें या अपनी निकटतम शाखा पर जाएं।</p>'+
            '<button onClick="closeWarning(2)" class="btn btn-primary" style="width:200px; margin-bottom:10px;" title="Confirm">पुष्टि करें</button>' +
            '<br>'+
            '<button onClick="closeWarning(1)" class="btn btn-danger" style="width:200px" title="Cancel">रद्द करें</button>' +
            '</div >' 
        );
    }
}

getSelectedLanguage = function getSelectedLanguage() {
    var isHindiSelected = $("#Hindi").prop('checked');
    if (isHindiSelected) {
        return 2;
    }
    return 1;
}

closeWarning = function closeWarning(lang) {
    var warningDiv = $('#languageWarning').length;
    window.selectedBotLanguage = lang;
    $('#webchat').css('opacity', '1')
    if (lang == 1) {
        $("#English").prop('checked', true);
        window.googleTransliterationControl.disableTransliteration();
    }
    if (!warningDiv == 0) {
        $('#languageWarning').css('display', 'none');
        $('#webchat').css('display', 'block');
    }
}
//close language radio pop-up if 
$(document).mouseup(function (e) {
    var languagesPopup = $("#languages");
    // if the target of the click isn't the container nor a descendant of the container
    if (!languagesPopup.is(e.target) && languagesPopup.has(e.target).length === 0) {
        languagesPopup.hide('fast');
    }
});
$(document).ready(function () {
    //loads languages
    window.languages = [];
    getLanguages();

    // previous word
    window.previousWord = null

    $(chatInputSelector).on('blur', function () {
        setTimeout(function () {
            window.scrollTo(0, document.body.clientHeight);
        }, 300);
    });
    $(chatInputSelector).on('focus', function () {
        setTimeout(function () {
            window.scrollTo(0, document.body.clientHeight);
        }, 300);
    });

    fetch('/Home/GetBotParams', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        }
    }).then(function (res) {
        return res.json();
    }).then(function (res) {
        window.directLine = window.WebChat.createDirectLine({
            domain: res.directLineUrl,
            token: res.directLineToken,
            webSocket: true
        });

        var onboardingCompleted = false;
        var subscription = directLine.activity$.filter(function (activity) {
            return activity.type === 'event' && activity.value === 'OnboardingCompleted';
        }).subscribe(function (_) {
            initializeAutoSuggest();
            displayCarousel();
            setTimeout(function () {
                $('#webchat div.main div').prepend('<button id="LanguageChange" onClick="changeLanguage()"><span>अ/A</span></button>');
            }, 500)

            onboardingCompleted = true;
            subscription.unsubscribe();
        });
        directLine.activity$.filter(function (activity) {
            setTimeout(function () {
                $(chatInputSelector).val('');
                if ($(chatInputSelector).autocomplete())
                    $(chatInputSelector).autocomplete().hide();

            }, 1);
            return activity.type === 'message';
        }).subscribe(function (activity) {

            activity.showFeedback = onboardingCompleted;

            if (activity.text && (activity.text.toLowerCase().startsWith("this is what i found on ") | activity.text.toLowerCase().startsWith("i did not find an exact answer but here is something similar"))) {
                activity.showFeedback = false;
                $(chatInputSelector).autocomplete().hide();

            }
        });

        if (/MSIE \d|Trident.*rv:/.test(navigator.userAgent)) {
            var styleOptions = {
                backgroundColor: '',
                hideUploadButton: true,
                botAvatarInitials: 'BOT',
                botAvatarImage: './botAvatar.png',
                botAvatarBackgroundColor: '#ffe8a3'
            };
            window.botUserId = res.userId;
            window.WebChat.renderWebChat({
                directLine: window.directLine,
                userID: res.userId,
                username: res.userId,
                locale: 'en-IN',
                styleOptions: styleOptions,
                resize: 'detect'
            }, document.getElementById('webchat'));
        }
    });
});

sendUserInputMessage = function sendUserInputMessage(msg_text) {
    window.directLine.postActivity({
        text: msg_text,
        textFormat: "plain",
        channelData: [{
            "context": window.current_Context
        }],
        type: "message",
        channelId: "webchat",
        from: {
            id: window.botUserId,
            role: "user"
        },
        locale: "en-IN",
        timestamp: new Date()
    }).subscribe(function () {
        window.suggested_items = []
        setTimeout(function () {
            $(chatInputSelector).val('');
        }, 200);
    });
};

var hideKeyboard = function () {
    document.activeElement.blur();
    $("input").blur();
};

var checkIfDomReady = function checkIfDomReady() {
    if ($('.main button').length == 0) {
        setTimeout(checkIfDomReady, 100);
        return;
    }
}

function displayCarousel() {
    var carousel = $("div#carousel-container").detach();
    $("div#webchat div.main").parent().prepend(carousel);
    carousel.show('fast');
    carousel.find("button.btn-suggestion").on('click', function () {
        setTimeout(function () {
            $(chatInputSelector).val('');
        }, 1);
        var text = $(this).data("msg-text");
        window.directLine.postActivity({
            text: text,
            textFormat: "plain",
            value: {
                "action": "menu",
                "text": text
            },
            type: "message",
            channelId: "webchat",
            from: {
                id: window.botUserId,
                role: "user"
            },
            locale: "en-IN",
            timestamp: new Date()
        }).subscribe();
    });
}

function initializeAutoSuggest() {
    $(chatInputSelector).autocomplete({
        orientation: 'top',
        // params - additional parameters to pass with the request, optional
        lookup: autoSuggestLookup,
        deferRequestBy: 150,
        noCache: false,
        minChars: 2,
        triggerSelectOnValidInput: true,
        tabDisabled: true,
        preventBadQueries: true,
        autoSelectFirst: false,
        onSearchComplete: function onSearchComplete(query, suggestions) {
            window.suggested_items = suggestions;
        },
        onSelect: function onSelect(suggestion) {
            window.current_Context = suggestion.context;
            sendUserInputMessage(suggestion.value);
            window.formattedResult = {
                suggestions: []
            };
        }
    }).bind("keypress", function (event) {

        //console.log(event)
        //console.log(event['keyCode'] == 32 && window.previousWord != 32)
        //if (event['keyCode'] == 32 && window.previousWord != 32) {

        //    var { lastWord, previousSentence } = getLastWord(event.target['value'])

        //    //make api call to translator
        //    var x = lastWord + "changed"
        //    event.target['value'] = previousSentence + " " + x

        //}
        //// after each key press
        //window.previousWord = event.keyCode

        $(chatInputSelector).on('blur', function () {
            setTimeout(function () {
                window.scrollTo(0, document.body.clientHeight);
            }, 300);
        });
        if (event.which == 13 && window.suggested_items) {
            if (window.suggested_items.length > 0) window.current_Context = window.suggested_items[0].context; else window.current_Context = "";
        }

    });
}

function getLastWord(sentence) {
    var wordArray = sentence.split(" ")
    var lastWord = wordArray[wordArray.length - 1]
    wordArray.pop()
    var previousSentence = wordArray.join(" ")
    //console.log(lastWord)
    return { lastWord, previousSentence }
}


function autoSuggestLookup(query, done) {
    var q = [{
        wildcard: {
            "Questions": "*" + query + "*"
        }
    }];

    if (query.split(" ").length > 1) {
        q = [];
        var words = query.split(" ");

        for (var i = 0; i < words.length; i++) {
            q.push({
                wildcard: {
                    "Questions": "*" + words[i] + "*"
                }
            });
        }
    }

    var dataToPost = {
        "from": 0,
        "size": 200,
        "query": {
            "bool": {
                "should": q
            }
        }
    };
    $.ajax({
        url: "/AutoSuggestion/Suggest",
        type: "POST",
        data: JSON.stringify({
            "Query": JSON.stringify(dataToPost)
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function success(result) {
            processAutoSuggestResults(result, done);
        },
        processData: false
    });
}

function processAutoSuggestResults(result, done) {
    if (!result) return;
    var empty_suggestions = {
        suggestions: []
    };

    if (result.hits.hits.length < 1) {
        done(empty_suggestions);
        return;
    }

    var suggestionCountToDisplay = 5;
    var suggestionList = result.hits.hits;

    if (suggestionList.length < 1) {
        done(empty_suggestions);
        return;
    }

    var curContext = (current_Context || '').toLowerCase();
    suggestionList = $.map(suggestionList, function (item, i) {
        var src = item._source;
        var itemContext = src.primary_context_and_keyword.split(',', 1)[0].toLowerCase();
        return {
            "value": src.Questions,
            "data": src.Questions,
            "context": itemContext
        };
    });
    var contextItems = [];
    var alternateItems = [];
    var itemsAlreadyDisplayed = [];

    for (var i = 0; i < suggestionList.length; i++) {
        // If we have 5 items (i.e., suggestionCountToDisplay) to display, then break out of the loop
        if (contextItems.length == suggestionCountToDisplay) break;
        var item = suggestionList[i]; // Do not display duplicate items. We do get duplicate items returned by ES from time-to-time!

        if (itemsAlreadyDisplayed.find(function (ele) {
            return ele === item.value;
        })) continue;

        if ($.trim(item.context).length > 0 && item.context == curContext) {
            contextItems.push(item);
        } else {
            alternateItems.push(item);
        }

        itemsAlreadyDisplayed.push(item.value);
    }

    done({
        suggestions: contextItems.length > 0 ? contextItems : alternateItems.slice(0, 5)
    });
}
