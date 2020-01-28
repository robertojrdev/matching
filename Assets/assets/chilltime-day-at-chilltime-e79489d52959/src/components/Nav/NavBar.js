import React from "react";
import "./NavBar.css";

class NavBar extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    const { page, changePage } = this.props;
    return (
      <div className="NavBar">
        <div className="Pages-wrapper">
          <div
            className={page === "game" ? "Page-item" : "Page-item active"}
            onClick={() => changePage("game")}
          >
            Card Game
          </div>
          <div
            className={
              page === "leaderboard" ? "Page-item" : "Page-item active"
            }
            onClick={() => changePage("leaderboard")}
          >
            Leaderboard
          </div>
        </div>
      </div>
    );
  }
}

export default NavBar;
