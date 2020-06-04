function initializeBot(directLineUrl) {
    window.directLine = null;
    var userId = "User-" + parseInt(Math.random() * 1000000);
    var userName = userId;

    fetch(directLineUrl + '/v3/directline/tokens/generate', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ userId: userId, }),
    })
        .then(res => res.json())
        .then(res => {
            window.directLine = window.WebChat.createDirectLine({
                domain: directLineUrl + '/v3/directline',
                token: res.token,
                webSocket: true
            });

            if (/MSIE \d|Trident.*rv:/.test(navigator.userAgent)) {
                var styleOptions = {
                    backgroundColor: '',
                    hideUploadButton: true,
                    botAvatarInitials: 'BOT',
                    botAvatarImage: './botAvatar.png',
                    botAvatarBackgroundColor: '#ffe8a3'
                };

                window.WebChat.renderWebChat({
                    directLine: window.directLine,
                    userID: userId,
                    username: userName,
                    locale: 'en-US',
                    styleOptions: styleOptions,
                    resize: 'detect'
                }, document.getElementById('webchat'));
            }
        });
}

window.current_Context = "undefined";

onSendButtonClick = function (e) {
    //Get
    var inputValue = $('#autocomplete-ajax').val();
    //sendMessage
    sendUserInputMessage(inputValue);
    //Set
    $('#autocomplete-ajax').val('');
}

sendUserInputMessage = function (e) {
    var activity = {
        text: e,
        textFormat: "plain",
        channelData: [{ "context": window.current_Context }],
        type: "message",
        channelId: "webchat",
        from: {
            id: "IndianBank_ChatBOT",
            role: "user"
        },
        locale: "en-US",
        timestamp: new Date()
    };
    window.directLine.postActivity(activity).subscribe(function () {
        console.log("message sent...");
    });
    // getCustomBotResponse(message);
}

function sendSuggestedMenuMessage(message) {
    var activity = {
        text: message,
        textFormat: "plain",
        channelData: [{ "context": window.current_Context }],
        type: "message",
        channelId: "webchat",
        from: {
            id: "IndianBank_ChatBOT",
            role: "user"
        },
        locale: "en-US",
        timestamp: new Date()
    };
    window.directLine.postActivity(activity).subscribe(function () {
        console.log("message sent...");
    });
}

$(document).ready(function () {
    setTimeout(function () {
        $("div.main").hide();
        var element = '<div class="main css-16qahhi css-qn8ts8 css-1g3yky9 css-1fe8kfc"><input id="autocomplete-ajax" aria-label="Type your message" placeholder="Type your message" type="text" value=""><div><button onclick="onSendButtonClick()" class="css-115fwte" title="Send" type="button"><img src="send-icon.png" style="width: 53%;"></button></div></div>';
        $(element).insertAfter($("div.main"));

        $('#autocomplete-ajax').autocomplete({
            orientation: 'top',
            // params - additional parameters to pass with the request, optional
            lookup: function (query, done) {

                var q = query.split(" ").length > 2 ? { match: { "Questions": query } } : { match_phrase: { "Questions": query } };

                var dataToPost =
                {
                    "from": 0, "size": 200,
                    "query": {
                        "bool": {
                            "should":
                                [
                                    q
                                ]
                        }
                    }
                };

                var empty_suggestions = { suggestions: [] };

                $.ajax({
                    url: "/AutoSuggestion/Suggest",
                    type: "POST",
                    data: JSON.stringify({ "Query": JSON.stringify(dataToPost) }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result != null) {
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

                                return { "value": src.Questions, "data": src.Questions, "context": itemContext };
                            });

                            /*console.log(curContext);
                            console.table(suggestionList);*/

                            var contextItems = [];
                            var alternateItems = [];

                            for (var i = 0; i < suggestionList.length; i++) {
                                // If we have 5 items (i.e., suggestionCountToDisplay) to display, then break out of the loop
                                if (contextItems.length == suggestionCountToDisplay)
                                    break;

                                var item = suggestionList[i];

                                if ($.trim(item.context).length > 0 && item.context == curContext) {
                                    contextItems.push(item);
                                } else {
                                    alternateItems.push(item);
                                }
                            }

                            done({
                                suggestions: contextItems.length > 0 ? contextItems : alternateItems.slice(0, 5)
                            });
                        }
                    },
                    error: function () {
                        //debugger;
                    },
                    processData: false
                });
            },
            // deferRequestBy - number of miliseconds to defer ajax request, default: 0
            deferRequestBy: 0,
            // ajaxSettings - any additional ajax settings that configure the jQuery Ajax request,
            // see: from: http://api.jquery.com/jquery.ajax/#jQuery-ajax-settings
            // END ajax settings
            // BEGIN general configuration settings
            // noCache - boolean value indicating whether to cache suggestion results, default: false
            noCache: false,
            // delimiter - string or RegExp, that splits input value and takes last part to as query for suggestions, useful when for example you need to fill list of comma separated values
            // delimiter: ??,

            // minChars - minimum number of characters required to trigger autosuggest, default: 1
            minChars: 2,

            // triggerSelectOnValidInput - boolean value indicating if select should be triggered if it matches suggestion, default: true
            triggerSelectOnValidInput: true,

            // preventBadQueries - boolean value indicating if it should prevent future ajax requests for queries with the same root if no results were returned, e.g. if Jam returns no suggestions, it will not fire for any future query that starts with Jam, default: true
            preventBadQueries: true,

            // autoSelectFirst - if set to true, first item will be selected when showing suggestions, default false
            autoSelectFirst: false,
            // BEGIN event handlers

            // onSearchStart: function (params) {}
            // called before ajax request, 'this' is bound to input element
            onSearchStart: function (params, e) {

            },
            // onSearchComplete: function (query, suggestions) {}
            // called after ajax response is processed, 'this' is bound to input element, suggestions is an array containing the results
            onSearchComplete: function (query, suggestions) {
                window.suggested_items = suggestions;
                //window.current_Context = suggestions[0].current_Context;
                //console.table(suggestions);
            },

            // onSelect: function (suggestion) {}
            // callback function invoked when user selects suggestion from the list, 'this' inside callback refers to input HtmlElement
            onSelect: function (suggestion) {
                //debugger;
                console.log("OLD CONTEXT:::::: " + window.current_Context);
                window.current_Context = suggestion.context;
                console.log("NEW CONTEXT:::: " + window.current_Context);

                //console.table(window.formattedResult.suggestions);
                sendUserInputMessage(suggestion.value);
                this.value = '';
                window.formattedResult = { suggestions: [] };
            },

            // onSearchError: function (query, jqXHR, textStatus, errorThrown)
            // {} called if ajax request fails, 'this' is bound to input element
            onSearchError: function (query, jqXHR, textStatus, errorThrown) {

            },

            // onHide: function (container) {}
            // called before container will be hidden
            onHide: function (container) {

            },

            onHint: function (hint) {

            },
            onInvalidateSelection: function () {

            }
        }).bind("keypress", function (event) {
            if (event.which == 13 && window.suggested_items) {
                console.log("OLD CONTEXT:::::: " + window.current_Context);
                if (window.suggested_items.length > 0)
                    window.current_Context = window.suggested_items[0].context;
                else
                    window.current_Context = "";

                console.log("NEW CONTEXT:::: " + window.current_Context);
            }
        });

        $("#autocomplete-ajax").keyup(function (event) {
            if (event.keyCode === 13) {
                sendUserInputMessage(event.target.value);
                this.value = '';
                window.formattedResult = { suggestions: [] };
            }
        });
    }, 1000);
});
