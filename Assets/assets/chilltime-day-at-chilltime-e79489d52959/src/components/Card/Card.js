import React from "react";
import styles from "./Card.css";

console.log("styles");
console.log(styles);
const Card = ({ cardType, cardState, onClick, styles }) => (
  <div className="card card_back" onClick={onClick}>
    <div>{cardType}</div>
    <div className="cardState">{cardState}</div>
  </div>
); 

export default Card;
