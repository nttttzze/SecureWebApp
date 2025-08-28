// import * as http from "../../lib/helpers/httpClient.js";
//  något fel i httpClient så satt "get" här
// Flytta ut till httpClient när allt fungerar

// export const get = async () => {
//   const url = "https://localhost:5001/api/TodoList";

//   try {
//     const urlResponse = await fetch(url);
//     if (urlResponse.ok) {
//       return await urlResponse.json();
//     } else {
//       throw new Error(
//         `Something went wrong ${urlResponse.status}, ${urlResponse.statusText}`
//       );
//     }
//   } catch (error) {
//     console.error(error);
//   }
// };

const todoList = document.querySelector("#list-container");

const initApp = () => {
  loadList();
};

const loadList = async () => {
  const response = await fetch("https://localhost:5001/api/TodoList", {
    method: "GET",
    mode: "cors",
    credentials: "include",
  });

  if (!response.ok) {
    throw new error(`HTTP error! Status: ${response.status}`);
  }
  const data = await response.json();
  todoList.innerHTML = "";

  console.log("Data: ", response);
  todoList.innerHTML = "";

  data.todo.forEach((list) => {
    todoList.appendChild(createHtml(list));
  });
};

export const createHtml = (todo) => {
  const li = document.createElement("li");
  li.classList.add("todo-card");
  li.innerHTML = `
    <div class="card">
      <span>${todo.task}
            
      </span>
      <button class="delete-btn" data-id="${todo.id}">X</button>
    </div>
  `;
  return li;
};
document.addEventListener("DOMContentLoaded", initApp);
