import "./App.css";
import Bar from "./component/Bar";
import Foo from "./component/Foo";
import { BrowserRouter, Route, Routes, Link } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <>
          <Link to="/foo"></Link>

          <Link to="/bar"></Link>
        </>
        <Routes>
          <Route path="/" element={<Foo />} index />
          <Route path="/foo" element={<Foo />} />
          <Route path="/bar" element={<Bar />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
