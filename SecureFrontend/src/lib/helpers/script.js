// const inputBox = document.getElementById("input-box");
// const listContainer = document.getElementById("list-container");
// const addButton = document.querySelector("button");

// addButton.addEventListener("click", addTask);

//----------------------------------------------------------
//               Bara test för TodoList
//----------------------------------------------------------

// function addTask() {
//   if (inputBox.value === "") {
//     alert("Write something");
//   } else {
//     let li = document.createElement("li");
//     li.innerHTML = inputBox.value;
//     listContainer.appendChild(li);
//     let span = document.createElement("span");
//     span.innerHTML = "\u00d7";
//     li.appendChild(span);
//   }
//   inputBox.value = "";
//   saveData();
// }

// listContainer.addEventListener(
//   "click",
//   function (e) {
//     if (e.target.tagName === "LI") {
//       e.target.classList.toggle("checked");
//       saveData();
//     } else if (e.target.tagName === "SPAN") {
//       e.target.parentElement.remove();
//       saveData();
//     }
//   },
//   false
// );

// function saveData() {
//   localStorage.setItem("data", listContainer.innerHTML);
// }
// function showTask() {
//   listContainer.innerHTML = localStorage.getItem("data");
// }
// showTask();

document.querySelector("#logoutBtn").addEventListener("click", logout);
export async function logout() {
  const response = await fetch("https://localhost:5001/api/auth/logout", {
    method: "POST",
    credentials: "include",
  });
  if (response.status === 401) {
    alert("You are not logged in!");
  } else if (response.status === 204) {
    alert("You are logged out");
  }

  console.log(response);
}
