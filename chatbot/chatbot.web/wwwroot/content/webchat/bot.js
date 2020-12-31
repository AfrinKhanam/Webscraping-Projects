var chatInputSelector = "input[data-id='webchat-sendbox-input']";
window.directLine = null;
window.current_Context = "undefined";
window.botUserId = null;
$(document).ready(function () {

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
            onboardingCompleted = true;
            subscription.unsubscribe();
        });
        directLine.activity$.filter(function (activity) {
            setTimeout(function () {
                $(chatInputSelector).val('');
                window.suggested_items = [];
            }, 1);
            return activity.type === 'message';
        }).subscribe(function (activity) {
            activity.showFeedback = onboardingCompleted;

            if (activity.text && (activity.text.toLowerCase().startsWith("this is what i found on ") | activity.text.toLowerCase().startsWith("i did not find an exact answer but here is something similar"))) {
                activity.showFeedback = false;
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
        if (event.which == 13 && window.suggested_items) {
            if (window.suggested_items.length > 0) window.current_Context = window.suggested_items[0].context; else window.current_Context = "";
        }

    });
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
