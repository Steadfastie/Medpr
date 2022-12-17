import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { catchError, delayWhen, interval, mergeMap, Observable, of, retry, retryWhen, tap, throwError, timer } from 'rxjs';
import { AuthService } from './auth.service';
import * as userActions from 'src/app/store/actions/auth.actions';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ErrorInterceptor implements HttpInterceptor {
  private toastr?: ToastrService;
  private router?: Router;
  constructor(private store: Store, private injector: Injector) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.toastr = this.injector.get(ToastrService);
    this.router = this.injector.get(Router);
    return next.handle(request).pipe(
      catchError(err => {
        const errorMessage = err.error?.detail || err.error?.title || err.status || "Something went wrong";
        switch (err.status) {
          case 401:
            this.store.dispatch(userActions.logout());
            this.toastr?.error(`Looks like it's time to signin again`, `Sorry`);
            break;
          case 403:
            this.toastr?.error(`This action is forbidden`, `Sorry`);
          break;
          case 500:
            this.toastr?.error(`${errorMessage}`, `Sorry`);
            break;
          default:
            this.toastr?.error(`${errorMessage}`, `Sorry`);
        }
        return throwError(() => errorMessage);
      })
    );
  }
}
