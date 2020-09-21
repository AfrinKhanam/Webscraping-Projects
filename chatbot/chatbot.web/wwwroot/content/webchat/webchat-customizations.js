"use strict";

function _defineProperty(obj, key, value) {
    if (key in obj) {
        Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true });
    } else {
        obj[key] = value;
    }
    return obj;
}

(async function () {
    'use strict';

    const {
        connectToWebChat,
        ReactWebChat
    } = window.WebChat;
    const {
        css
    } = window.Glamor;
    const styleOptions = {
        backgroundColor: '',
        hideUploadButton: true,
        botAvatarInitials: 'BOT',
        botAvatarImage: './botAvatar.png',
        botAvatarBackgroundColor: '#e2aa42ad'
    };
    const ACTIVITY_WITH_FEEDBACK_CSS = css({
        position: 'relative',
        '& > .activity': {
            paddingLeft: 0
        },
        '& > .button-bar': {
            paddingLeft: 50,
            listStyleType: 'none',
            margin: '0 0 0 10px',
            position: 'relative',
            '& > li': {
                display: 'inline-block'
            },
            '& > li > button': {
                background: 'White',
                border: 'solid 0px #E6E6E6',
                marginBottom: 2,
                padding: '2px 5px 5px',
                '&:hover': {
                    opacity: 0.50,
                    cursor: 'pointer'
                }
            }
        }
    });
    const ATTACHMENT_FEEDBACK = css({
        padding: 10,
        margin: 0,
        textAlign: 'left',
        minHeight: 20,
        position: 'relative',
        '& > button': {
            fontFamily: '"Calibri", "Helvetica Neue", "Arial", "sans-serif"',
            padding: '2px 10px',
            margin: '3px 3px',
            fontSize: '16px',
            color: 'black',
            background: '#f1f0f0',
            borderRadius: '5px',
            weight: '400',
            '&:hover': {
                opacity: 0.50,
                cursor: 'pointer'
            }
        }
    });
    window.current_Context = "";
    const store = window.WebChat.createStore();

    class IntegraHeroCard extends React.Component {
        constructor(...args) {
            super(...args);

            _defineProperty(this, "sendMessageButton", e => {
                return store.dispatch({
                    type: 'WEB_CHAT/SEND_MESSAGE',
                    payload: {
                        text: e
                    }
                });
            });
        }

        render() {
            const {
                props
            } = this;
            const items = [];

            for (const button of props.buttons) {
                items.push(React.createElement("button", {
                    onClick: () => this.sendMessageButton(button.value),
                    key: button.title
                }, button.title));
            }

            return React.createElement("div", {
                className: ATTACHMENT_FEEDBACK
            }, items);
        }

    }

    class ActivityWithFeedback extends React.Component {
        constructor() {
            super();

            _defineProperty(this, "sendFeedback", feedBack => {
                fetch('/Report/UpdateFeedback', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        ActivityId: this.props.replyId,
                        ResonseFeedback: feedBack
                    })
                });
            });

            _defineProperty(this, "handleDownvoteButton", () => {
                this.setState({
                    upvote_class: 'feedback-done',
                    downvote_class: 'reaction-downvote feedback-done',
                    disabled: 'disabled'
                });
                this.sendFeedback(-1);
            });

            _defineProperty(this, "handleUpvoteButton", () => {
                this.setState({
                    upvote_class: 'reaction-upvote feedback-done',
                    downvote_class: 'feedback-done',
                    disabled: 'disabled'
                });
                this.sendFeedback(1);
            });

            this.state = {
                upvote_class: '',
                downvote_class: '',
                disabled: false
            };
        }

        render() {
            const {
                props
            } = this;

            if (props.activity.showFeedback) {
                return React.createElement("div", {
                    className: ACTIVITY_WITH_FEEDBACK_CSS
                }, React.createElement("div", {
                    className: "activity"
                }, props.children), React.createElement("ul", {
                    className: "button-bar"
                }, React.createElement("li", null, React.createElement("button", {
                    className: this.state.upvote_class,
                    disabled: false,
                    onClick: this.handleUpvoteButton
                }, React.createElement("span", {
                    className: "glyphicon glyphicon-thumbs-up",
                    "aria-hidden": "true",
                    title: "Satisfied with BOT's response"
                }))), React.createElement("li", null, React.createElement("button", {
                    className: this.state.downvote_class,
                    disabled: false,
                    onClick: this.handleDownvoteButton
                }, React.createElement("span", {
                    className: "glyphicon glyphicon-thumbs-down",
                    "aria-hidden": "true",
                    title: "Not satisfied"
                })))));
            } else {
                return React.createElement("div", {
                    className: ACTIVITY_WITH_FEEDBACK_CSS
                }, React.createElement("div", {
                    className: "activity"
                }, props.children));
            }
        }

    }

    const ConnectedActivityWithFeedback = connectToWebChat(({
        postActivity
    }) => ({
        postActivity
    }))(props => React.createElement(ActivityWithFeedback, props));

    const checkDirectLine = function checkDirectLine() {
        if (!window.directLine) {
            setTimeout(checkDirectLine, 100);
            return;
        }

        ReactDOM.render(React.createElement(ReactWebChat, {
            activityMiddleware: activityMiddleware,
            directLine: window.directLine,
            styleOptions: styleOptions,
            store: store
        }), document.getElementById('webchat'));
        document.querySelector('#webchat > *').focus();
    };

    const activityMiddleware = () => next => card => {
        if (card.activity.type === 'message' && card.activity.from.role === 'bot') {
            return children => React.createElement(ConnectedActivityWithFeedback, {
                key: card.activity.id,
                activity: card.activity,
                activityID: card.activity.id,
                replyId: card.activity.replyToId
            }, next(card)(children));
        } else {
            return next(card);
        }
    };

    setTimeout(function () {
        checkDirectLine();
    }, 100);
})().catch(err => console.error(err));