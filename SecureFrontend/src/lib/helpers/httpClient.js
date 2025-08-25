import { config } from "./config";

const user = JSON.parse(sessionStorage.getItem("user"));

export const get = async (endpoint) => {
  const url = `${config.apiUrl}/${endpoint}`;

  try {
    const urlResponse = await fetch(url);
    if (urlResponse.ok) {
      return await urlResponse.json();
    } else {
      throw new Error(
        `Something went wrong ${urlResponse.status}, ${urlResponse.statusText}`
      );
    }
  } catch (error) {
    console.error(error);
  }
};

export const authPost = async (endpoint, data) => {
  const url = `${config.apiUrl}/${endpoint}`;

  try {
    const urlResponse = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "bearer " + user.token,
      },
      body: JSON.stringify(data),
    });

    if (urlResponse.ok) {
      return await urlResponse.json();
    } else if (urlResponse.status === 401) {
      alert("You are not logged in");
      location.reload();
    } else {
      throw new Error(
        `POST misslyckades: ${urlResponse.status}, ${urlResponse.statusText}`
      );
    }
  } catch (error) {
    console.error("Something went wrong on POST", error);
    return null;
  }
};

export const post = async (endpoint, data) => {
  const url = `${config.apiUrl}/${endpoint}`;

  try {
    const urlResponse = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (urlResponse.ok) {
      return await urlResponse.json();
    } else {
      throw new Error(
        `POST misslyckades: ${urlResponse.status}, ${urlResponse.statusText}`
      );
    }
  } catch (error) {
    console.error("Something went wrong on POST", error);
    return null;
  }
};
