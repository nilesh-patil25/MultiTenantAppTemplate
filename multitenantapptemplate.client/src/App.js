import { useEffect } from "react";
import { BrowserRouter, useLocation, Route, Routes } from "react-router-dom";
import "./App.css";
import Bar from "./component/Bar";
import Foo from "./component/Foo";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <AppContent />
      </BrowserRouter>
    </div>
  );
}

function AppContent() {
  const location = useLocation();

  useEffect(() => {
    const rootElement = document.getElementById("root");
    if (rootElement) {
      if (location.pathname === "/foo") {
        document.body.style.backgroundColor = "#a78436";
      } else if (location.pathname === "/bar") {
        document.body.style.backgroundColor = "#2d545e";
      } else if (location.pathname === "/") {
        document.body.style.backgroundColor = "#a78436";
      }
    }
  }, [location]);

  return (
    <>
      {/* <Link to="/foo">Foo</Link>
      <Link to="/bar">Bar</Link> */}
      <Routes>
        <Route path="/" element={<Foo />} index />
        <Route path="/foo" element={<Foo />} />
        <Route path="/bar" element={<Bar />} />
      </Routes>
    </>
  );
}

export default App;
