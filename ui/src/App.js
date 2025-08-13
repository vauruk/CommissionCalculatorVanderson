import "./App.css";
import { useState, useRef } from "react";

function App() {
  const [totalFcamaraCommission, setTotalFcamaraCommission] = useState(0);
  const [totalCompetitorCommission, setTotalCompetitorCommission] = useState(0);
  const localSalesCountRef = useRef(null);
  const foreignSalesCountRef = useRef(null);
  const averageSaleAmountRef = useRef(null);

  const URL = "https://localhost:5000/Commision";

  /**
   * @author Vanderson Vauruk
   * @description This function is called when the form is submitted. It gathers the input values
   * @param {*} event
   */
  async function calculate(event) {
    event.preventDefault();

    const requestBody = {
      localSalesCount: localSalesCountRef.current.value,
      foreignSalesCount: foreignSalesCountRef.current.value,
      averageSaleAmount: averageSaleAmountRef.current.value,
    };
    try {
      const response = await fetch(URL, {
        method: "POST",
        headers: {
          Accept: "text/json",
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*",
        },
        body: JSON.stringify(requestBody),
      });
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
      setTotalFcamaraCommission(data.fCamaraCommissionAmount);
      setTotalCompetitorCommission(data.competitorCommissionAmount);
    } catch (error) {
      console.error("Error fetching commission data:", error);
    }
  }

  /**
   * @author Vanderson Vauruk
   * @description This function formats a number as a currency string in GBP format.
   * It uses the Intl.NumberFormat API to format the number.
   * @param {*} value
   * @returns
   */
  function formatCurrency(value) {
    return new Intl.NumberFormat("en-GB", {
      style: "currency",
      currency: "GBP",
    }).format(value);
  }

  return (
    <div className="App">
      <header className="App-header">
        <div></div>
        <form onSubmit={calculate}>
          <label for="localSalesCount">Local Sales Count</label>
          <input ref={localSalesCountRef} name="localSalesCount" />
          <br />

          <label for="foreignSalesCount">Foreign Sales Count</label>
          <input ref={foreignSalesCountRef} name="foreignSalesCount" />
          <br />

          <label for="averageSaleAmount">Average Sale Amount</label>
          <input ref={averageSaleAmountRef} name="averageSaleAmount" />
          <br />

          <button type="submit">Calculate</button>
        </form>
      </header>

      <div>
        <h3>Results</h3>
        <p>
          Total FCamara commission: {formatCurrency(totalFcamaraCommission)}
        </p>
        <p>
          Total Competitor commission:{" "}
          {formatCurrency(totalCompetitorCommission)}
        </p>
      </div>
    </div>
  );
}

export default App;
