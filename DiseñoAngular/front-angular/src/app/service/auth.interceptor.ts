import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './acceso.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);

  const token = auth.getToken();
  
  const clonedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
      'Contend-type': 'applicacion/json',
    },
  });
  return next(clonedReq)
};
