import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentsComponent } from './pages/appointments/appointments.component';
import { SigninComponent } from './pages/auth/signin/signin.component';
import { SignupComponent } from './pages/auth/signup/signup.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import { DrugsComponent } from './pages/drugs/drugs.component';
import { ErrorComponent } from './pages/error/error.component';
import { HomeComponent } from './pages/home/home/home.component';
import { PrescriptionsComponent } from './pages/prescriptions/prescriptions.component';
import { FamiliesComponent } from './pages/user/families/families.component';
import { ProfileComponent } from './pages/user/profile/profile.component';
import { SettingsComponent } from './pages/user/settings/settings.component';
import { UserComponent } from './pages/user/user/user.component';
import { VaccinationsComponent } from './pages/vaccinations/vaccinations.component';
import { VaccinesComponent } from './pages/vaccines/vaccines.component';
import { AdminGuardService } from './services/auth/admin.guard';
import { AuthGuardService } from './services/auth/auth.guard';
import { AuthReverseGuardService } from './services/auth/auth.reverse.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuardService] },
  { path: 'drugs', component: DrugsComponent, canActivate: [AuthGuardService] },
  { path: 'vaccines', component: VaccinesComponent, canActivate: [AuthGuardService] },
  { path: 'doctors', component: DoctorsComponent, canActivate: [AuthGuardService] },
  { path: 'appointments', component: AppointmentsComponent, canActivate: [AuthGuardService] },
  { path: 'appointments/:id', component: AppointmentsComponent, canActivate: [AuthGuardService] },
  { path: 'vaccinations', component: VaccinationsComponent, canActivate: [AuthGuardService] },
  { path: 'vaccinations/:id', component: VaccinationsComponent, canActivate: [AuthGuardService] },
  { path: 'prescriptions', component: PrescriptionsComponent, canActivate: [AuthGuardService] },
  { path: 'prescriptions/:id', component: PrescriptionsComponent, canActivate: [AuthGuardService] },
  { path: 'user', component: SettingsComponent,
    children: [
      {
        path: "profile",
        title: "user profile",
        component: ProfileComponent,
      },
      {
        path: "families",
        title: "user families",
        component: FamiliesComponent,
      }
  ], canActivate: [AuthGuardService]},
  { path: 'error', component: ErrorComponent },
  { path: 'users', component: UserComponent, canActivate: [AdminGuardService] },
  { path: 'signin', component: SigninComponent, canActivate: [AuthReverseGuardService] },
  { path: 'signup', component: SignupComponent, canActivate: [AuthReverseGuardService] },
  { path: '**', component: HomeComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
