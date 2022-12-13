import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { AppRoutingModule } from './app-routing.module';
import { SignupComponent } from './pages/auth/signup/signup.component';
import { SigninComponent } from './pages/auth/signin/signin.component';

import { TrimPipe } from './pipes/trim';

import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/app.states';
import { AuthEffects } from './store/effects/auth.effects';
import { AuthService } from './services/auth/auth.service';
import { TokenInterceptor } from './services/auth/token.interceptor';

import { DrugsComponent } from './pages/drugs/drugs.component';
import { DrugCardComponent } from './pages/drugs/drug.card/drug.card.component';
import { CreateDrugComponent } from './pages/drugs/create.drug/create.drug.component';
import { EditDrugComponent } from './pages/drugs/edit.drug/edit.drug.component';
import { DrugInfoComponent } from './pages/drugs/drug.info/drug.info.component';

import { DoctorsComponent } from './pages/doctors/doctors.component';
import { DoctorCardComponent } from './pages/doctors/doctor.card/doctor.card.component';
import { CreateDoctorComponent } from './pages/doctors/create.doctor/create.doctor.component';
import { EditDoctorComponent } from './pages/doctors/edit.doctor/edit.doctor.component';
import { DoctorInfoComponent } from './pages/doctors/doctor.info/doctor.info.component';

import { VaccinesComponent } from './pages/vaccines/vaccines.component';
import { VaccineCardComponent } from './pages/vaccines/vaccine.card/vaccine.card.component';
import { CreateVaccineComponent } from './pages/vaccines/create.vaccine/create.vaccine.component';
import { EditVaccineComponent } from './pages/vaccines/edit.vaccine/edit.vaccine.component';
import { VaccineInfoComponent } from './pages/vaccines/vaccine.info/vaccine.info.component';

import { AppointmentsComponent } from './pages/appointments/appointments.component';
import { AppointmentCardComponent } from './pages/appointments/appointment.card/appointment.card.component';
import { CreateAppointmentComponent } from './pages/appointments/create.appointment/create.appointment.component';
import { EditAppointmentComponent } from './pages/appointments/edit.appointment/edit.appointment.component';

import { VaccinationsComponent } from './pages/vaccinations/vaccinations.component';
import { VaccinationCardComponent } from './pages/vaccinations/vaccination.card/vaccination.card.component';
import { CreateVaccinationComponent } from './pages/vaccinations/create.vaccination/create.vaccination.component';
import { EditVaccinationComponent } from './pages/vaccinations/edit.vaccination/edit.vaccination.component';

import { PrescriptionsComponent } from './pages/prescriptions/prescriptions.component';
import { PrescriptionCardComponent } from './pages/prescriptions/prescription.card/prescription.card.component';
import { CreatePrescriptionComponent } from './pages/prescriptions/create.prescription/create.prescription.component';
import { EditPrescriptionComponent } from './pages/prescriptions/edit.prescription/edit.prescription.component';

import { ErrorComponent } from './pages/error/error.component';
import { UserComponent } from './pages/user/user/user.component';
import { ErrorInterceptor } from './services/auth/error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { HomeComponent } from './pages/home/home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SigninComponent,
    SignupComponent,
    TrimPipe,

    DrugsComponent,
    DrugCardComponent,
    CreateDrugComponent,
    EditDrugComponent,
    DrugInfoComponent,

    DoctorsComponent,
    DoctorCardComponent,
    CreateDoctorComponent,
    EditDoctorComponent,
    DoctorInfoComponent,

    VaccinesComponent,
    VaccineCardComponent,
    CreateVaccineComponent,
    EditVaccineComponent,
    VaccineInfoComponent,

    AppointmentsComponent,
    AppointmentCardComponent,
    CreateAppointmentComponent,
    EditAppointmentComponent,

    VaccinationsComponent,
    VaccinationCardComponent,
    CreateVaccinationComponent,
    EditVaccinationComponent,

    PrescriptionsComponent,
    PrescriptionCardComponent,
    CreatePrescriptionComponent,
    EditPrescriptionComponent,

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
      timeOut: 7000,
      extendedTimeOut: 3000,
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      progressBar: true,
      maxOpened: 5,
      autoDismiss: true,
      positionClass: 'toast-bottom-right',
    }),
    MaterialModule,
    StoreModule.forRoot(reducers, {}),
    EffectsModule.forRoot([AuthEffects]),
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
