import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';

import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { SignupComponent } from './pages/auth/signup/signup.component';
import { SigninComponent } from './pages/auth/signin/signin.component';

import { TrimPipe } from './pipes/trim';

import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/app.states';
import { AuthEffects } from './store/effects/auth.effects';
import { AuthService } from './services/auth/auth.service';
import { AuthGuardService } from './services/auth/auth-guard.service';
import { TokenInterceptor } from './services/auth/token.interceptor';

import { DrugsComponent } from './pages/drugs/drugs.component';
import { DrugCardComponent } from './pages/drugs/drug.card/drug.card.component';
import { CreateDrugComponent } from './pages/drugs/create.drug/create.drug.component';
import { EditDrugComponent } from './pages/drugs/edit.drug/edit.drug.component';
import { ErrorComponent } from './pages/error/error.component';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { UserComponent } from './pages/user/user/user.component';

@NgModule({
  declarations: [
    AppComponent,
    SigninComponent,
    SignupComponent,
    TrimPipe,

    DrugsComponent,
    DrugCardComponent,
    CreateDrugComponent,
    EditDrugComponent,
    ErrorComponent,
    UserComponent,
    SigninComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    StoreModule.forRoot(reducers, {}),
    EffectsModule.forRoot([AuthEffects]),
    RouterModule.forRoot([
      // { path: '', component: DrugsComponent, canActivate: [AuthGuardService] },
      { path: '', component: DrugsComponent},
      { path: 'user', component: UserComponent},
      { path: 'signin', component: SigninComponent },
      { path: 'signup', component: SignupComponent },
    ]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
  ],
  providers: [
    AuthService,
    // AuthGuardService,
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: TokenInterceptor,
    //   multi: true
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
