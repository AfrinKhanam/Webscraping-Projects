if (!(/MSIE \d|Trident.*rv:/.test(navigator.userAgent))) {
    (async function () {
        'use strict';

        const { connectToWebChat, ReactWebChat } = window.WebChat;
        const { css } = window.Glamor;
        const styleOptions = {
            backgroundColor: '',
            hideUploadButton: true,
            botAvatarInitials: 'BOT',
            botAvatarImage: './botAvatar.png',
            botAvatarBackgroundColor: '#e2aa42ad'
        };


        const ACTIVITY_WITH_FEEDBACK_CSS = css({
            minHeight: 60,
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
                        cursor: 'pointer',
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
                    cursor: 'pointer',
                }

            }
        });

        window.current_Context = "";

        const store = window.WebChat.createStore();

        class IntegraHeroCard extends React.Component {

            sendMessageButton = (e) => {

                return store.dispatch({
                    type: 'WEB_CHAT/SEND_MESSAGE',
                    payload: { text: e }
                });
            }
            render() {
                const { props } = this;
                const items = []
                for (const button of props.buttons) {
                    items.push(
                        <button onClick={() => this.sendMessageButton(button.value)} key={button.title}>{button.title}</button>)
                }
                return (
                    <div className={ATTACHMENT_FEEDBACK}>
                        {items}
                    </div>
                );
            }
        }

        const attachmentMiddleware = () => next => card => {
            switch (card.attachment.contentType) {
                case 'application/vnd.microsoft.card.hero': {
                    console.log(card.attachment.contentType)
                    console.log(card.attachment)
                    console.log(card)
                    return (
                        <IntegraHeroCard buttons={card.attachment.content.buttons} />
                    );
                }

                default:
                    return next(card);
            }
        };

        const REACTION_DISABLED = css({
            enabled: false
        });

        const REACTION_SELECTED = css({
            backgroundColor: 'red'
        });

        class ActivityWithFeedback extends React.Component {

            constructor() {
                super();

                this.state = {
                    upvote_class: '',
                    downvote_class: '',
                    disabled: false
                };
            }

            sendFeedback = (feedBack) => {
                fetch('/Report/UpdateFeedback', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        ActivityId: this.props.replyId,
                        ResonseFeedback: feedBack,
                    })
                })
            }

            handleDownvoteButton = () => {
                this.setState({ upvote_class: 'feedback-done', downvote_class: 'reaction-downvote feedback-done', disabled: 'disabled' });

                this.sendFeedback(-1);
            }

            handleUpvoteButton = () => {
                this.setState({ upvote_class: 'reaction-upvote feedback-done ', downvote_class: 'feedback-done', disabled: 'disabled' });

                this.sendFeedback(1);
            }

            render() {
                const { props } = this;

                return (
                    <div className={ACTIVITY_WITH_FEEDBACK_CSS}>
                        <div className="activity">{props.children}</div>
                        <ul className="button-bar">
                            <li>
                                <button className={this.state.upvote_class} disabled={false} onClick={this.handleUpvoteButton}><span className="glyphicon glyphicon-thumbs-up" aria-hidden="true" title="Satisfied with BOT's response"></span></button>
                            </li>
                            <li>
                                <button className={this.state.downvote_class} disabled={false} onClick={this.handleDownvoteButton}><span className="glyphicon glyphicon-thumbs-down" aria-hidden="true" title="Not satisfied"></span></button>
                            </li>
                        </ul>

                    </div>
                );
            }
        }

        const ConnectedActivityWithFeedback = connectToWebChat(({ postActivity }) => ({ postActivity }))(props => (
            <ActivityWithFeedback {...props} />
        ));
        let excludedText = [`Hi! My name is ADYA 😃.
        Welcome to Indian Bank.
        I am your virtual assistant, here to assist you with all your banking queries 24x7`, "Please enter your name to get me started"];

        const checkDirectLine = function () {

            if (!window.directLine) {
                setTimeout(checkDirectLine, 100);
                return;
            }

            console.log('chatbot initialized');
            window.ReactDOM.render(
                <ReactWebChat // attachmentMiddleware={attachmentMiddleware}
                    activityMiddleware={activityMiddleware}
                    directLine={window.directLine}
                    styleOptions={styleOptions}
                    store={store} />,
                document.getElementById('webchat')
            );

            document.querySelector('#webchat > *').focus();
        }

        const activityMiddleware = () => next => card => {
            if (card.activity.from.role === 'bot' && !excludedText.includes(card.activity.text)) {
                return children => (
                    <ConnectedActivityWithFeedback key={card.activity.id} activityID={card.activity.id} replyId={card.activity.replyToId}>
                        {next(card)(children)}
                    </ConnectedActivityWithFeedback>
                );
            } else {
                return next(card);
            }
        };

        setTimeout(function () {
            checkDirectLine();
        }, 100);

    })().catch(err => console.error(err));
}