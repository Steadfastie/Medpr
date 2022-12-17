import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Family } from 'src/app/models/family';
import { FamiliesActionsService } from 'src/app/services/families/families.actions.service';
import { FamiliesService } from 'src/app/services/families/families.service';
import { selectStateUser } from 'src/app/store/app.states';
import { MembersActionsService } from 'src/app/services/members/members.actions.service';

@Component({
  selector: 'family-control',
  templateUrl: './family.control.component.html',
  styleUrls: ['./family.control.component.scss']
})
export class FamilyControlComponent implements OnInit {
  @Input() family?: Family;
  creatorName?: string;
  currentUserId?: string;
  isCurrentUserCreator: boolean = false;
  isCurrentUserAdmin: boolean = false;
  showSpinner: boolean = false;

  constructor(private fb: FormBuilder,
    private store: Store,
    private toastr: ToastrService,
    private familiesService: FamiliesService,
    private actions: FamiliesActionsService,
    private memberActions: MembersActionsService,
    ){}

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.currentUserId = authUser?.userId;
      // Check if current user is creator
      if (this.family?.creator == authUser!.userId) {
        this.isCurrentUserCreator = true;
      }
      // Check if current user is an admin of the family
      let currentUserMember = this.family!.members?.
        filter(member => member.user?.['id'] == authUser!.userId)
      if (currentUserMember![0].isAdmin){
        this.isCurrentUserAdmin = true;
      }
    });

    // Set creator name
    let creator = this.family?.members?.find(member => member.user!["id"] == this.family?.creator)
    if (creator?.user?.fullName){
      this.creatorName = creator?.user?.fullName;
    } else {
      let dogIndex = creator?.user?.login.lastIndexOf('@');
      this.creatorName = creator?.user?.login.substring(0, dogIndex);
    }

    this.memberActions.memberResponseListner().subscribe(memberFromAction => {
      const presentMember = this.family?.members!.find((presentMember) => {
        return presentMember.id === memberFromAction.id;
      })
      if (!presentMember) {
        this.family?.members!.push(memberFromAction);
      }
      else{
        this.family?.members!.splice(this.family?.members!.indexOf(presentMember), 1, memberFromAction);
      }
    });

    // Update list if member was removed
    this.memberActions.memberDeleteListner().subscribe(memberId => {
      const presentMember = this.family?.members!.find((member) => {
        return member.id === memberId;
      })
      this.family?.members!.splice(this.family.members!.indexOf(presentMember!), 1);
    });
  }

  deleteFamily(){
    if (this.isCurrentUserCreator){
      this.showSpinner = true;
      this.familiesService.delete(this.family?.id!).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`${this.family!.surname} removed`, `Success`);
          this.actions.emitFamilyDelete(this.family!.id);
        },
        error: (err) => {
          this.showSpinner = false;
          this.toastr.warning(`${this.family!.surname} still persist`, `Failed`);
          console.log(`${err.message}`);
        },
      })
    }

  }
}
