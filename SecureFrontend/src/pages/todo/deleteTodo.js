// Flytta ut till httpClient när allt fungerar

export const taskDelete = async (id) => {
  const url = `https://localhost:5001/api/todoList/delete/${id}`;

  try {
    const urlResponse = await fetch(url, {
      method: "DELETE",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        // Authorization: "bearer " + user.token,
      },
    });

    if (urlResponse.ok) {
      return await urlResponse.json();
    } else if (urlResponse.status === 401) {
      alert("Unauthorized");
      location.reload();
    } else {
      throw new Error(
        `DELETE misslyckades: ${urlResponse.status}, ${urlResponse.statusText}`
      );
    }
  } catch (error) {
    console.log("Something went wrong on DELETE", error);
    return null;
  }
};

document.addEventListener("click", async (e) => {
  if (e.target.classList.contains("delete-btn")) {
    const id = e.target.dataset.id;
    const result = await taskDelete(id);

    if (result) {
      e.target.closest("li").remove();
    }
  }
});
