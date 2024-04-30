import "./App.css";
import About from "./component/About";
import Home from "./component/Home";
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <>
          <Link to="/">Foo</Link>
          {" || "}
          <Link to="/about">Bar</Link>
        </>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
