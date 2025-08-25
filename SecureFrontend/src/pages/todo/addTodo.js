// import { post } from "../../lib/helpers/httpClient.js";
import { createHtml } from "./todo.js";

// Flytta ut till httpClient när allt fungerar
export const post = async (data) => {
  const url = "https://localhost:5001/api/TodoList/addTask";

  try {
    console.log("POST sending:", data);
    const urlResponse = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!urlResponse.ok) {
      const errorText = await urlResponse.text();
      console.error("POST failed:", errorText);
      return null;
    }

    return await urlResponse.json();
  } catch (error) {
    console.error("Something went wrong on POST", error);
    return null;
  }
};

const form = document.getElementById("todo-form");
const todoList = document.getElementById("list-container");

const addTask = async (e) => {
  e.preventDefault();

  const taskData = new FormData(e.target);
  const taskInfo = Object.fromEntries(taskData.entries());

  const payload = { Task: taskInfo.task };

  const response = await post(payload);
  console.log("results:", response, taskInfo, payload);

  if (response) {
    todoList.appendChild(createHtml(response));
  }
};
form.addEventListener("submit", addTask);
