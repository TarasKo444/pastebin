interface Paste {
  id: string;
  text: string;
  title: string;
  expirationTime: string;
  createdAt: string;
}

interface ApiError {
  status: number;
  errors: string[];
}