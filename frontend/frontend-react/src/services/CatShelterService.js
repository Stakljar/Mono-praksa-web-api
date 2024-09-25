import axiosInstance from "../axios/axiosInstance";

export async function addCatShelter(data) {
  try {
    return await axiosInstance.post("/cat_shelters/add", data)
  }
  catch (error) {
    return error
  }
}

export async function getCatShelter(id) {
  try {
    return await axiosInstance.get(`/cat_shelters/${id}`)
  }
  catch (error) {
    return error
  }
}

export async function getCatShelters() {
  try {
    return await axiosInstance.get(`/cat_shelters`,
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

export async function getCatSheltersWithoutCats() {
  try {
    return await axiosInstance.get(`/cat_shelters/without_cats`)
  }
  catch (error) {
    return error
  }
}

export async function deleteCatShelter(id) {
  try {
    return await axiosInstance.delete(`/cat_shelters/delete/${id}`)
  }
  catch (error) {
    return error
  }
}

export async function updateCatShelter(id, data) {
  try {
    return await axiosInstance.put(`/cat_shelters/update/${id}`, data)
  }
  catch (error) {
    return error
  }
}
