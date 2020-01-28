import React from "react";
import "./Game.css";
import Card from "../Card/Card";
import table from "../../data/table.json";

class Game extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      cardArray: []
    };
    //function needs to be bound to be used in child component (Card.js)
    this.clickCard = this.clickCard.bind(this);
  }

  componentWillMount() {
    console.log("DEBUG: componentWillMount");
    this.generateCardArray();
    this.shuffleFunction();
  }

  generateCardArray() {
    console.log("DEBUG: -> generateCardArray");
    var _cardArray = this.state.cardArray;
    for (var i = 0; i < table.length; i++) {
      _cardArray[i] = {
        cardType: table[i].cardType,
        cardState: table[i].cardState
      };
    }
    this.setState({ cardArray: _cardArray });
  }

  shuffleFunction() {
    console.log("DEBUG: -> shuffleFunction");
    var _cardArray = this.state.cardArray;
    _cardArray.sort(() => Math.random() - 0.5);
    this.setState({ cardArray: _cardArray });
  }

  clickCard(_position) {
    var _cardArray = this.state.cardArray;
    console.log("Clicked:" + _position);
    console.log(_cardArray[_position]);
    if (_cardArray[_position].cardState === "INVISIBLE") {
      _cardArray[_position].cardState = "VISIBLE";
    } else {
      _cardArray[_position].cardState = "INVISIBLE";
    }
    this.setState({ cardArray: _cardArray });
  }

  renderCardTable() {
    return this.state.cardArray.map((card, index) => {
      const { cardType, cardState } = card;
      return (
        <Card
          key={index}
          cardType={cardType}
          cardState={cardState}
          onClick={() => this.clickCard(index)}
        />
      );
    });
  }

  render() {
    console.log(this.state.cardArray);
    return <div className="Table-wrapper">{this.renderCardTable()}</div>;
  }
}

export default Game;
