const initApp = () => {};
const add = document.getElementById("register-form");

const user = JSON.parse(sessionStorage.getItem("user"));

// const test = document.getElementById("testBtn");
// test.addEventListener("click", function () {
//   alert("test");
// });

// Flytta ut till httpClient när allt fungerar
// Ändra till cookie ist för session

console.log("test");
const register = async (e) => {
  e.preventDefault();
  const registerData = new FormData(e.target);
  const registerInfo = Object.fromEntries(registerData.entries());

  try {
    var url = "https://localhost:5001/api/auth/register";
    const response = await fetch(url, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(registerInfo),
    });
    console.log("Data: ", registerData, "Info: ", registerInfo);

    if (response.ok) {
      const result = await response.json();
      alert("User registered");

      console.log(result);
    }
  } catch (error) {
    console.log("Error", error);
  }
};
document.addEventListener("DOMContentLoaded", initApp);
document.querySelector("#register-form").addEventListener("submit", register);
