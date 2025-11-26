import { HttpInterceptorFn, HttpRequest, HttpHandler } from '@angular/common/http';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  if (!token) {
    console.log("Pas de token, requête non authentifiée:", req.url);
  } else {
    console.log("TOKEN FROM INTERCEPTOR:", token);
  }

  const cloned = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;
  return next(cloned);
};
