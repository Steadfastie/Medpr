import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users/users.service';
import { selectStateUser } from 'src/app/store/app.states';

import * as userActions from 'src/app/store/actions/auth.actions';
import { UsersActionsService } from 'src/app/services/users/users.actions.service';

@Component({
  selector: 'edit-profile',
  templateUrl: './edit.profile.component.html',
  styleUrls: ['./edit.profile.component.scss'],
})
export class EditProfileComponent implements OnInit {
  @Input() profile?: User;
  name?: string;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  openEdit: boolean = false;
  errorMessage?: string;


  constructor(
    private fb: FormBuilder,
    private store: Store,
    private usersService: UsersService,
    private userActions: UsersActionsService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    // Get user from store
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.profile = authUser;
      let dogIndex = this.profile?.login.lastIndexOf('@');
      this.name = this.profile?.login.substring(0, dogIndex);
      this.initialize();
    });

    // Get additional user information
    this.usersService.getUserById(this.profile!.userId!).pipe().subscribe((user) => {
      this.profile = user;
      this.profile.userId = user["id"];
      if (user.fullName){
        this.name = this.profile.fullName;
      }
      this.initialize();
    });

    this.profileForm.controls.password.disable();
    this.profileForm.controls.newPassword.disable();
    this.profileForm.controls.fullName.disable();
    this.profileForm.controls.dateOfBirth.disable();
  }

  profileForm = this.fb.group({
    password: ['', [Validators.minLength(5), Validators.maxLength(30)]],
    newPassword: ['', [Validators.minLength(5), Validators.maxLength(30)]],
    fullName: ['', [Validators.minLength(1), Validators.maxLength(30)]],
    dateOfBirth: [''],
  });

  initialize() {
    if (this.profile) {
      this.profileForm.setValue({
        password: null,
        newPassword: null,
        fullName: null,
        dateOfBirth: null,
      });

      if (this.profile.fullName) {
        this.profileForm.patchValue({
          fullName: this.profile.fullName,
        });
      }

      if (this.profile.dateOfBirth) {
        this.profileForm.patchValue({
          dateOfBirth: this.profile.dateOfBirth,
        });
      }
    }
  }

  edit() {
    if (!this.showSpinner && this.profileForm.valid) {
      this.showSpinner = true;

      let initialProfile = this.profile;
      initialProfile!.password = '00000';
      let modifiedProfile = Object.assign({}, initialProfile);

      // If password was changed
      if (
        this.profileForm.value.password != null &&
        this.profileForm.value.password == this.profileForm.value.newPassword
      ) {
        modifiedProfile!.password = this.profileForm.value.password!;
        modifiedProfile!.newPassword = this.profileForm.value.newPassword!;
      }

      // If fullName was changed
      if (this.profileForm.value.fullName != null) {
        modifiedProfile!.fullName = this.profileForm.value.fullName!;
      }

      // If date of birth was provided
      if (this.profileForm.value.dateOfBirth != null) {
        let date = new Date(this.profileForm.value.dateOfBirth!);
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();

        let dateTime = year + '-' + month + '-' + day;
        modifiedProfile!.dateOfBirth = dateTime;
      }

      if (JSON.stringify(modifiedProfile) !== JSON.stringify(initialProfile)) {
        this.usersService.patch(modifiedProfile!).pipe()
          .subscribe({
            next: (profile) => {
              this.showSpinner = false;
              this.profile = profile;
              this.initialize();
              this.toastr.success(`Success`, `${profile.login} updated`);
              this.editToggle();
              this.userActions.emitUserResponse(profile);
            },
            error: (err) => {
              this.showSpinner = false;
              console.log(`${err}`);
              this.toastr.error(
                `${modifiedProfile!.login} is still the same`, `Failed`,
              );
              this.errorMessage = 'Could not modify profile';
            },
          });
      } else {
        this.showSpinner = false;
      }
    }
  }

  remove() {
    if (!this.showSpinner) {
      this.showSpinner = true;
      this.usersService
        .delete(this.profile!.userId!)
        .pipe()
        .subscribe({
          next: () => {
            this.showSpinner = false;
            this.toastr.success(`Success`, `${this.profile!.login} removed`);
            this.store.dispatch(userActions.logout());
          },
          error: (err) => {
            this.toastr.warning(
              `Failed`,
              `${this.profile!.login} still persist`
            );
            console.log(`${err.message}`);
          },
        });
    }
  }

  editToggle() {
    if (!this.showSpinner && this.openEdit) {
      this.openEdit = false;
      this.profileForm.controls.password.disable();
      this.profileForm.controls.newPassword.disable();
      this.profileForm.controls.fullName.disable();
      this.profileForm.controls.dateOfBirth.disable();
      this.initialize();
    } else if (!this.showSpinner && !this.openEdit) {
      this.openEdit = true;
      this.profileForm.controls.password.enable();
      this.profileForm.controls.newPassword.enable();
      this.profileForm.controls.fullName.enable();
      this.profileForm.controls.dateOfBirth.enable();
    }
  }
}
