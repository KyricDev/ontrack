var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var GoalTitle = function (_React$Component) {
    _inherits(GoalTitle, _React$Component);

    function GoalTitle(props) {
        _classCallCheck(this, GoalTitle);

        var _this = _possibleConstructorReturn(this, (GoalTitle.__proto__ || Object.getPrototypeOf(GoalTitle)).call(this, props));

        _this.changeTitle = _this.changeTitle.bind(_this);
        return _this;
    }

    _createClass(GoalTitle, [{
        key: "changeTitle",
        value: function changeTitle(e) {
            this.props.title(e.target.value);
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement("input", { type: "text", placeholder: "Enter Title", onChange: this.changeTitle });
        }
    }]);

    return GoalTitle;
}(React.Component);

var GoalContent = function (_React$Component2) {
    _inherits(GoalContent, _React$Component2);

    function GoalContent(props) {
        _classCallCheck(this, GoalContent);

        var _this2 = _possibleConstructorReturn(this, (GoalContent.__proto__ || Object.getPrototypeOf(GoalContent)).call(this, props));

        _this2.changeDescription = _this2.changeDescription.bind(_this2);
        return _this2;
    }

    _createClass(GoalContent, [{
        key: "changeDescription",
        value: function changeDescription(e) {
            this.props.description(e.target.value);
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement("textarea", { row: "5", column: "150", placeholder: "Enter Description", onChange: this.changeDescription });
        }
    }]);

    return GoalContent;
}(React.Component);

export var AddGoalContainer = function (_React$Component3) {
    _inherits(AddGoalContainer, _React$Component3);

    function AddGoalContainer(props) {
        _classCallCheck(this, AddGoalContainer);

        var _this3 = _possibleConstructorReturn(this, (AddGoalContainer.__proto__ || Object.getPrototypeOf(AddGoalContainer)).call(this, props));

        _this3.state = {
            title: "",
            description: ""
        };
        _this3.updateTitle = _this3.updateTitle.bind(_this3);
        _this3.updateDescription = _this3.updateDescription.bind(_this3);4;
        _this3.submit = _this3.submit.bind(_this3);
        return _this3;
    }

    _createClass(AddGoalContainer, [{
        key: "updateTitle",
        value: function updateTitle(e) {
            this.setState({ title: e });
        }
    }, {
        key: "updateDescription",
        value: function updateDescription(e) {
            this.setState({ description: e });
        }
    }, {
        key: "submit",
        value: function submit(e) {
            fetch("https://localhost:5001/api/User/AddRoadMapItem", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    title: this.state.title,
                    description: this.state.description
                })
            }).then(function (response) {
                return response.json();
            }).then(function (data) {
                return console.log(data);
            });
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement(
                "div",
                null,
                React.createElement(GoalTitle, { title: this.updateTitle }),
                React.createElement("br", null),
                React.createElement("br", null),
                React.createElement(GoalContent, { description: this.updateDescription }),
                React.createElement("br", null),
                React.createElement(
                    "button",
                    { type: "submit", onClick: this.submit },
                    "Submit"
                )
            );
        }
    }]);

    return AddGoalContainer;
}(React.Component);