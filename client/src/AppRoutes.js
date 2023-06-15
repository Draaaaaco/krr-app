// import { Counter } from "./components/Counter";
// import { FetchData } from "./components/FetchData";
import Home from "./components/Home";
import Manual from "./components/Manual";
const AppRoutes = [
  {
    index: true,
    element: <Home />
  }, {
    path: '/manual',
    element: <Manual />
  }
];

export default AppRoutes;
