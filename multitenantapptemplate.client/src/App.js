import "./App.css";
import About from "./component/About";
import Home from "./component/Home";
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <>
          <Link to="/foo">Foo</Link>
          {" || "}
          <Link to="/bar">Bar</Link>
        </>
        <Routes>
          <Route path="/foo" element={<Home />} />
          <Route path="/bar" element={<About />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
