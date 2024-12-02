import { useEffect, useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";

type WeatherForecast = {
  date: string;
  temperatureC: number;
  summary: string;
  temperatureF: number;
};
function App() {
  const [count, setCount] = useState(0);

  const [weather, setWeather] = useState<WeatherForecast[] | null>(null);

  useEffect(() => {
    fetch(`${import.meta.env.VITE_API_URL}/weatherforecast`)
      .then((res) => res.json())
      .then((data) => setWeather(data));
  }, []);

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>

      <h1>Weather {import.meta.env.VITE_API_URL}</h1>
      <ul>
        {weather?.map((forecast) => (
          <li key={forecast.date}>
            <strong>{forecast.date}</strong>: {forecast.summary} (
            {forecast.temperatureC}°C/{forecast.temperatureF}°F)
          </li>
        ))}
      </ul>
    </>
  );
}

export default App;
