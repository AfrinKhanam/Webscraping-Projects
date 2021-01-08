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

    window.current_Context = "";

    const store = window.WebChat.createStore(
        {},
        ({ dispatch }) => next => action => {
            const actionTypesToHandle = [
                "DIRECT_LINE/INCOMING_ACTIVITY",
                "DIRECT_LINE/POST_ACTIVITY_FULFILLED",
                "DIRECT_LINE/INCOMING_ACTIVITY",
                "WEB_CHAT/SET_SUGGESTED_ACTIONS"]

            if (actionTypesToHandle.includes(action.type)) {
                var lastChild = document.querySelector('ul[role="list"]').lastChild;

                if (lastChild) {
                    lastChild.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
            return next(action);
        }
    );

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
            $.ajax({
                type: "POST",
                url: '/Report/UpdateFeedback',
                data: {
                    ActivityId: this.props.replyId,
                    ResonseFeedback: feedBack,
                }
            });
        }

        handleDownvoteButton = () => {
            this.setState({ upvote_class: 'feedback-done', downvote_class: 'reaction-downvote feedback-done', disabled: 'disabled' });

            this.sendFeedback(-1);
        }

        handleUpvoteButton = () => {
            this.setState({ upvote_class: 'reaction-upvote feedback-done', downvote_class: 'feedback-done', disabled: 'disabled' });

            this.sendFeedback(1);
        }

        render() {
            const { props } = this;

            if (props.activity.showFeedback) {
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
            } else {
                return (
                    <div className={ACTIVITY_WITH_FEEDBACK_CSS}>
                        <div className="activity">{props.children}</div>
                    </div>
                );
            }
        }
    }

    const ConnectedActivityWithFeedback = connectToWebChat(({ postActivity }) => ({ postActivity }))(props => (
        <ActivityWithFeedback {...props} />
    ));

    const checkDirectLine = function () {

        if (!window.directLine) {
            setTimeout(checkDirectLine, 100);
            return;
        }

        window.ReactDOM.render(
            <ReactWebChat
                activityMiddleware={activityMiddleware}
                directLine={window.directLine}
                styleOptions={styleOptions}
                store={store} />,
            document.getElementById('webchat')
        );

        document.querySelector('#webchat > *').focus();
    }

    const activityMiddleware = () => next => card => {
        if (card.activity.type === 'message' && card.activity.from.role === 'bot') {
            return children => (
                <ConnectedActivityWithFeedback key={card.activity.id} activity={card.activity} activityID={card.activity.id} replyId={card.activity.replyToId}>
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