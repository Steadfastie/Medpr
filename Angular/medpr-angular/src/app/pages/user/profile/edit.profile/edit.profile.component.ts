import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/models/user';
import { UsersActionsService } from 'src/app/services/users/users.actions.service';
import { UsersService } from 'src/app/services/users/users.service';


@Component({
  selector: 'edit-profile',
  templateUrl: './edit.profile.component.html',
  styleUrls: ['./edit.profile.component.scss'],
})
export class EditProfileComponent implements OnInit {
  @Input() profile?: User;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  openEdit: boolean = false;
  errorMessage?: string;

  constructor(
    private fb: FormBuilder,
    private usersService: UsersService,
    private toastr: ToastrService,
    private actions: UsersActionsService
  ) {}

  ngOnInit(): void {
    this.initialize();

    if (this.profile?.fullName) {
      this.profileForm.patchValue({
        fullName: this.profile.fullName,
      });
    }
    if (this.profile?.dateOfBirth) {
      this.profileForm.patchValue({
        dateOfBirth: this.profile.dateOfBirth,
      });
    }

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

      const initialProfile = this.profile;
      let modifiedProfile = Object.assign({}, initialProfile);

      // If password was changed
      if (
        this.profileForm.value.password != "" &&
        this.profileForm.value.password == this.profileForm.value.newPassword
      ) {
        modifiedProfile!.password = this.profileForm.value.password!;
        modifiedProfile!.newPassword = this.profileForm.value.newPassword!;
      } else {
        modifiedProfile.password = "00000";
        modifiedProfile.newPassword = "00000";
      }

      // If fullName was changed
      if (this.profileForm.value.fullName != "") {
        modifiedProfile!.fullName = this.profileForm.value.fullName!;
      } else {
        modifiedProfile.fullName = "";
      }

      // If date of birth was provided
      if (this.profileForm.value.dateOfBirth != "") {
        let date = new Date(this.profileForm.value.dateOfBirth!);
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();

        let dateTime = year + '-' + month + '-' + day;
        modifiedProfile!.dateOfBirth = dateTime;
      } else {
        modifiedProfile.dateOfBirth = "0000-00-00";
      }

      modifiedProfile.role = "Default";

      if (JSON.stringify(modifiedProfile) !== JSON.stringify(initialProfile)) {
        this.usersService.patch(modifiedProfile!).pipe()
          .subscribe({
            next: (profile) => {
              this.showSpinner = false;
              this.actions.emitUserResponse(profile);
              this.toastr.success(`Success`, `${profile.login} updated`);
              this.editToggle();
            },
            error: (err) => {
              this.showSpinner = false;
              console.log(`${err}`);
              this.toastr.error(
                `Failed`,
                `${modifiedProfile!.login} is still the same`
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
            this.actions.emitUserDelete(this.profile!.userId!);
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
