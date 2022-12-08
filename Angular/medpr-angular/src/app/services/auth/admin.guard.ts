import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { selectUserRole } from 'src/app/store/app.states';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService implements CanActivate {
  isAdmin: boolean = false;

  constructor(
    public authService: AuthService,
    public router: Router,
    private store: Store,
    private toastr: ToastrService
  ) {
    this.store.select(selectUserRole).pipe()
      .subscribe(status => status == "admin" ?
        this.isAdmin = true : this.isAdmin = false
      );
  }

  canActivate(): Observable<boolean|UrlTree>|Promise<boolean|UrlTree>|boolean|UrlTree {
    if (this.isAdmin){
      return true;
    }
    this.toastr.warning(`Access denied`);
    this.router.navigateByUrl('/error');
    return false;
  }
}
