"use strict";

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

function _instanceof2(left, right) { if (right != null && typeof Symbol !== "undefined" && right[Symbol.hasInstance]) { return !!right[Symbol.hasInstance](left); } else { return left instanceof right; } }

function _instanceof(left, right) {
    if (right != null && typeof Symbol !== "undefined" && right[Symbol.hasInstance]) {
        return !!right[Symbol.hasInstance](left);
    } else {
        return _instanceof2(left, right);
    }
}

function _typeof(obj) {
    "@babel/helpers - typeof";

    if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") {
        _typeof = function _typeof(obj) {
            return typeof obj;
        };
    } else {
        _typeof = function _typeof(obj) {
            return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj;
        };
    }

    return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
    if (!_instanceof(instance, Constructor)) {
        throw new TypeError("Cannot call a class as a function");
    }
}

function _defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor) descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
    }
}

function _createClass(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties(Constructor, staticProps);
    return Constructor;
}

function _inherits(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
        throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
        constructor: {
            value: subClass,
            writable: true,
            configurable: true
        }
    });
    if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
    _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
        o.__proto__ = p;
        return o;
    };

    return _setPrototypeOf(o, p);
}

function _createSuper(Derived) {
    var hasNativeReflectConstruct = _isNativeReflectConstruct();

    return function _createSuperInternal() {
        var Super = _getPrototypeOf(Derived),
            result;

        if (hasNativeReflectConstruct) {
            var NewTarget = _getPrototypeOf(this).constructor;

            result = Reflect.construct(Super, arguments, NewTarget);
        } else {
            result = Super.apply(this, arguments);
        }

        return _possibleConstructorReturn(this, result);
    };
}

function _possibleConstructorReturn(self, call) {
    if (call && (_typeof(call) === "object" || typeof call === "function")) {
        return call;
    }

    return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
    if (self === void 0) {
        throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
}

function _isNativeReflectConstruct() {
    if (typeof Reflect === "undefined" || !Reflect.construct) return false;
    if (Reflect.construct.sham) return false;
    if (typeof Proxy === "function") return true;

    try {
        Date.prototype.toString.call(Reflect.construct(Date, [], function () { }));
        return true;
    } catch (e) {
        return false;
    }
}

function _getPrototypeOf(o) {
    _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
        return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf(o);
}

function _defineProperty(obj, key, value) {
    if (key in obj) {
        Object.defineProperty(obj, key, {
            value: value,
            enumerable: true,
            configurable: true,
            writable: true
        });
    } else {
        obj[key] = value;
    }

    return obj;
}

_asyncToGenerator( /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
    'use strict';

    var _window$WebChat, connectToWebChat, ReactWebChat, css, styleOptions, ACTIVITY_WITH_FEEDBACK_CSS, ATTACHMENT_FEEDBACK, store, ActivityWithFeedback, ConnectedActivityWithFeedback, checkDirectLine, activityMiddleware;

    return regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
            switch (_context.prev = _context.next) {
                case 0:
                    _window$WebChat = window.WebChat, connectToWebChat = _window$WebChat.connectToWebChat, ReactWebChat = _window$WebChat.ReactWebChat;
                    css = window.Glamor.css;
                    styleOptions = {
                        backgroundColor: '',
                        hideUploadButton: true,
                        botAvatarInitials: 'BOT',
                        botAvatarImage: './botAvatar.png',
                        botAvatarBackgroundColor: '#e2aa42ad'
                    };
                    ACTIVITY_WITH_FEEDBACK_CSS = css({
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
                    ATTACHMENT_FEEDBACK = css({
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
                    store = window.WebChat.createStore({}, function (_ref) {
                        var dispatch = _ref.dispatch;
                        return function (next) {
                            return function (action) {
                                var actionTypesToHandle = ["DIRECT_LINE/INCOMING_ACTIVITY", "DIRECT_LINE/POST_ACTIVITY_FULFILLED", "DIRECT_LINE/INCOMING_ACTIVITY", "WEB_CHAT/SET_SUGGESTED_ACTIONS"];

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
                            };
                        };
                    });

                    ActivityWithFeedback = /*#__PURE__*/function (_React$Component) {
                        _inherits(ActivityWithFeedback, _React$Component);

                        var _super = _createSuper(ActivityWithFeedback);

                        function ActivityWithFeedback() {
                            var _this;

                            _classCallCheck(this, ActivityWithFeedback);

                            _this = _super.call(this);

                            _defineProperty(_assertThisInitialized(_this), "sendFeedback", function (feedBack) {
                                fetch('/Report/UpdateFeedback', {
                                    method: 'POST',
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    body: JSON.stringify({
                                        ActivityId: _this.props.replyId,
                                        ResonseFeedback: feedBack
                                    })
                                });
                            });

                            _defineProperty(_assertThisInitialized(_this), "handleDownvoteButton", function () {
                                _this.setState({
                                    upvote_class: 'feedback-done',
                                    downvote_class: 'reaction-downvote feedback-done',
                                    disabled: 'disabled'
                                });

                                _this.sendFeedback(-1);
                            });

                            _defineProperty(_assertThisInitialized(_this), "handleUpvoteButton", function () {
                                _this.setState({
                                    upvote_class: 'reaction-upvote feedback-done',
                                    downvote_class: 'feedback-done',
                                    disabled: 'disabled'
                                });

                                _this.sendFeedback(1);
                            });

                            _this.state = {
                                upvote_class: '',
                                downvote_class: '',
                                disabled: false
                            };
                            return _this;
                        }

                        _createClass(ActivityWithFeedback, [{
                            key: "render",
                            value: function render() {
                                var props = this.props;

                                if (props.activity.showFeedback) {
                                    return /*#__PURE__*/React.createElement("div", {
                                        className: ACTIVITY_WITH_FEEDBACK_CSS
                                    }, /*#__PURE__*/React.createElement("div", {
                                        className: "activity"
                                    }, props.children), /*#__PURE__*/React.createElement("ul", {
                                        className: "button-bar"
                                    }, /*#__PURE__*/React.createElement("li", null, /*#__PURE__*/React.createElement("button", {
                                        className: this.state.upvote_class,
                                        disabled: false,
                                        onClick: this.handleUpvoteButton
                                    }, /*#__PURE__*/React.createElement("span", {
                                        className: "glyphicon glyphicon-thumbs-up",
                                        "aria-hidden": "true",
                                        title: "Satisfied with BOT's response"
                                    }))), /*#__PURE__*/React.createElement("li", null, /*#__PURE__*/React.createElement("button", {
                                        className: this.state.downvote_class,
                                        disabled: false,
                                        onClick: this.handleDownvoteButton
                                    }, /*#__PURE__*/React.createElement("span", {
                                        className: "glyphicon glyphicon-thumbs-down",
                                        "aria-hidden": "true",
                                        title: "Not satisfied"
                                    })))));
                                } else {
                                    return /*#__PURE__*/React.createElement("div", {
                                        className: ACTIVITY_WITH_FEEDBACK_CSS
                                    }, /*#__PURE__*/React.createElement("div", {
                                        className: "activity"
                                    }, props.children));
                                }
                            }
                        }]);

                        return ActivityWithFeedback;
                    }(React.Component);

                    ConnectedActivityWithFeedback = connectToWebChat(function (_ref2) {
                        var postActivity = _ref2.postActivity;
                        return {
                            postActivity: postActivity
                        };
                    })(function (props) {
                        return /*#__PURE__*/React.createElement(ActivityWithFeedback, props);
                    });

                    checkDirectLine = function checkDirectLine() {
                        if (!window.directLine) {
                            setTimeout(checkDirectLine, 100);
                            return;
                        }

                        window.ReactDOM.render( /*#__PURE__*/React.createElement(ReactWebChat, {
                            activityMiddleware: activityMiddleware,
                            directLine: window.directLine,
                            styleOptions: styleOptions,
                            store: store
                        }), document.getElementById('webchat'));
                        document.querySelector('#webchat > *').focus();
                    };

                    activityMiddleware = function activityMiddleware() {
                        return function (next) {
                            return function (card) {
                                if (card.activity.type === 'message' && card.activity.from.role === 'bot') {
                                    return function (children) {
                                        return /*#__PURE__*/React.createElement(ConnectedActivityWithFeedback, {
                                            key: card.activity.id,
                                            activity: card.activity,
                                            activityID: card.activity.id,
                                            replyId: card.activity.replyToId
                                        }, next(card)(children));
                                    };
                                } else {
                                    return next(card);
                                }
                            };
                        };
                    };

                    setTimeout(function () {
                        checkDirectLine();
                    }, 100);

                case 12:
                case "end":
                    return _context.stop();
            }
        }
    }, _callee);
}))().catch(function (err) {
    return console.error(err);
});