import axios from "axios";

const API = process.env.NEXT_PUBLIC_API_URL;
axios.defaults.baseURL = API;

export class api {
  static getPaste(id: string) {
    return axios.get(`paste/${id}`);
  }
  static createPaste(paste: PastePostDto) {
    return axios.post("paste", paste);
  }
}