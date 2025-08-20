import * as http from "../../lib/helpers/httpClient.js";
const todoList = document.querySelector("#list");

const initApp = () => {
  console.log("test test pls work");
  loadList();
};

const loadList = async () => {
  const result = await http.get("TodoList");
  console.log("Data: ", result);

  result.todo.forEach((list) => {
    todoList.appendChild(createHtml(list));
  });
};

const createHtml = (list) => {
  const display = document.createElement("div");

  let html = `<section class="card">

    <div class="card-body">
    <h3 class="p-name"> ${list.Id} </h3>
    <h3 class="p-name"> ${list.Task} </h3>
    </div>
    `;

  display.innerHTML = html;
  return display;
};
document.addEventListener("DOMContentLoaded", initApp);

// ???????????????????????????????????? npgot fel. Kanske i httpClient
