const initApp = () => {
  console.log("Hellooo test test");
};

const add = document.getElementById("login-form");

const login = async (e) => {
  e.preventDefault();
  const loginData = new FormData(e.target);
  const loginInfo = Object.fromEntries(loginData.entries());

  try {
    // var url = "https://localhost:5001/api";

    const response = await fetch(
      "https://localhost:5001/api/login?useCookies=true",
      {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(loginInfo),
      }
    );
    console.log("Data:", loginData, "Info:", loginInfo);

    if (response.ok) {
      // const result = await response.json();
      // console.log(result);
      // const user = {
      //   userName: result.userName,
      //   token: result.token,
      // };
      // sessionStorage.setItem("user", JSON.stringify(user));
      alert("You are logged in!");
    } else {
      console.log(response.status, response.statusText);
    }
  } catch (error) {
    console.log("Error:", error);
  }
};

document.addEventListener("DOMContentLoaded", initApp);
document.querySelector("#login-form").addEventListener("submit", login);
