import axiosInstance from "../axios/axiosInstance";

export async function addCat(data) {
  try {
    return await axiosInstance.post("/cats/add", data)
  }
  catch (error) {
    return error
  }
}

export async function getCat(id) {
  try {
    return await axiosInstance.get(`/cats/${id}`)
  }
  catch (error) {
    return error
  }
}

export async function getCats() {
  try {
    return await axiosInstance.get(`/cats`,
      {
        params: {

        }
      }
    )
  }
  catch (error) {
    return error
  }
}

export async function deleteCat(id) {
  try {
    return await axiosInstance.delete(`/cats/delete/${id}`)
  }
  catch (error) {
    return error
  }
}

export async function updateCat(id, data) {
  try {
    return await axiosInstance.put(`/cats/update/${id}`, data)
  }
  catch (error) {
    return error
  }
}
