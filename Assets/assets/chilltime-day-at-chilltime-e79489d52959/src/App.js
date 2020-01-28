import React from "react";
import "./App.css";
import NavBar from "./components/Nav/NavBar";
import Game from "./components/Game/Game";
import LeaderBoard from "./components/LeaderBoard/LeaderBoard";

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      page: "game"
    };
    //function needs to be bound to be used in child component (NavBar.js)
    this.changePage = this.changePage.bind(this);
  }

  changePage(page) {
    this.setState({
      page
    });
  }

  render() {
    const { page } = this.state;
    return (
      <div className="App">
        <NavBar page={page} changePage={this.changePage} />
        <div className="App-header">
          {page === "game" && <Game />}
          {page === "leaderboard" && <LeaderBoard />}
        </div>
      </div>
    );
  }
}

export default App;
