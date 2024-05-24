import axios from "axios";

const API = process.env.NEXT_PUBLIC_API_URL;
axios.defaults.baseURL = API;

export class Api {
  static getPaste(id: string) {
    return axios.get(`paste/${id}`);
  }
}