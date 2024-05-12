export const fetchApi = (url: string) => fetch(process.env.NEXT_PUBLIC_API_URL + url);
