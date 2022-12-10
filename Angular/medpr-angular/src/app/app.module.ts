import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

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
import { AuthGuardService } from './services/auth/auth.guard';
import { TokenInterceptor } from './services/auth/token.interceptor';

import { DrugsComponent } from './pages/drugs/drugs.component';
import { DrugCardComponent } from './pages/drugs/drug.card/drug.card.component';
import { CreateDrugComponent } from './pages/drugs/create.drug/create.drug.component';
import { EditDrugComponent } from './pages/drugs/edit.drug/edit.drug.component';

import { DoctorsComponent } from './pages/doctors/doctors.component';
import { DoctorCardComponent } from './pages/doctors/doctor.card/doctor.card.component';
import { CreateDoctorComponent } from './pages/doctors/create.doctor/create.doctor.component';
import { EditDoctorComponent } from './pages/doctors/edit.doctor/edit.doctor.component';

import { AppointmentsComponent } from './pages/appointments/appointments.component';
import { AppointmentCardComponent } from './pages/appointments/appointment.card/appointment.card.component';
import { CreateAppointmentComponent } from './pages/appointments/create.appointment/create.appointment.component';
import { EditAppointmentComponent } from './pages/appointments/edit.appointment/edit.appointment.component';

import { ErrorComponent } from './pages/error/error.component';
import { UserComponent } from './pages/user/user/user.component';
import { AdminGuardService } from './services/auth/admin.guard';
import { AuthReverseGuardService } from './services/auth/auth.reverse.guard';
import { ErrorInterceptor } from './services/auth/error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { DoctorInfoComponent } from './pages/doctors/doctor.info/doctor.info.component';
import { HomeComponent } from './pages/home/home/home.component';

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

    DoctorsComponent,
    DoctorCardComponent,
    CreateDoctorComponent,
    EditDoctorComponent,
    DoctorInfoComponent,

    AppointmentsComponent,
    AppointmentCardComponent,
    CreateAppointmentComponent,
    EditAppointmentComponent,

    ErrorComponent,
    UserComponent,
    SigninComponent,
    DoctorInfoComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      disableTimeOut: true,
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      maxOpened: 5,
      autoDismiss: true,
      positionClass: 'toast-bottom-right',
    }),
    MaterialModule,
    StoreModule.forRoot(reducers, {}),
    EffectsModule.forRoot([AuthEffects]),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, canActivate: [AuthGuardService] },
      { path: 'drugs', component: DrugsComponent, canActivate: [AuthGuardService] },
      { path: 'doctors', component: DoctorsComponent, canActivate: [AuthGuardService] },
      { path: 'appointments', component: AppointmentsComponent, canActivate: [AuthGuardService] },
      { path: 'error', component: ErrorComponent },
      { path: 'user', component: UserComponent, canActivate: [AdminGuardService] },
      { path: 'signin', component: SigninComponent, canActivate: [AuthReverseGuardService] },
      { path: 'signup', component: SignupComponent, canActivate: [AuthReverseGuardService] },
    ]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
  ],
  providers: [
    AuthService,
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
