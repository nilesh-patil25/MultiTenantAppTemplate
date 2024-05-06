import { useEffect,useState} from "react";
import { BrowserRouter, useLocation, Route, Routes } from "react-router-dom";
import "./App.css";
import Bar from "./component/Bar";
import Foo from "./component/Foo";
import axios from 'axios';


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
  const [backgroundColor, setBackgroundColor] = useState('');

  useEffect(() => {
    const fetchBackgroundColor = async () => {
      try {
        const response = await axios.get(`https://localhost:7213/api/Tenant/GetTenantTheme?tenantName=${location.pathname.slice(1)}`);
        setBackgroundColor(response.data.backgroundColor);
      } catch (error) {
        console.error('Error fetching background color:', error);
      }
    };

    fetchBackgroundColor();
  }, [location]);

  useEffect(() => {
    document.body.style.backgroundColor = backgroundColor;
  }, [backgroundColor]);

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
