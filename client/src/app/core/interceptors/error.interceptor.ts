import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor{

  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    throw next.handle(req).pipe(
      catchError((error) => {
        if(error)
        {
          if(error.status === 404)
          {
            this.router.navigateByUrl('/not-found');
          }
          else if(error.status === 401)
          {
            this.router.navigateByUrl('/un-authenticated');
          }
          else if(error.status === 500)
            {
              this.router.navigateByUrl('/server-error');
            }
        }
        return throwError(() => new Error(error));
      })
    );
  }

}