import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { catchError, delayWhen, interval, mergeMap, Observable, of, retry, retryWhen, tap, throwError, timer } from 'rxjs';
import { AuthService } from './auth.service';
import * as userActions from 'src/app/store/actions/auth.actions';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class ErrorInterceptor implements HttpInterceptor {
  private toastr?: ToastrService;
  constructor(private store: Store, private injector: Injector) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.toastr = this.injector.get(ToastrService);
    return next.handle(request).pipe(
      catchError(err => {
        const errorMessage = err.error?.detail || err.error?.title || err.status;
        switch (err.status) {
          case 401:
            this.toastr?.error(`Sorry`, `Looks like it's time to signin again`);
            this.store.dispatch(userActions.logout());
            break;
          case 500:
            this.toastr?.error(`${errorMessage}`);
            break;
          default:
            this.toastr?.error(`${errorMessage}`);
        }
        return throwError(() => errorMessage);
      }),
      retry(1)
    );
  }
}
