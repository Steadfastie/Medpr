import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Family } from 'src/app/models/family';
import { selectStateUser } from 'src/app/store/app.states';
import { MembersActionsService } from 'src/app/services/members/members.actions.service';
import { MembersService } from 'src/app/services/members/members.service';
import { Guid } from 'guid-typescript';
import { Member } from 'src/app/models/member';

@Component({
  selector: 'join-family',
  templateUrl: './join.component.html',
  styleUrls: ['./join.component.scss']
})
export class JoinFamilyComponent implements OnInit {
  @Input() family?: Family;
  currentUserId?: string;
  isCurrentUserIn: boolean = false;
  isCurrentUserCreator: boolean = false;
  creatorName?: string;

  constructor(
    private store: Store,
    private toastr: ToastrService,
    private membersService: MembersService,
    private membersActions: MembersActionsService,
  ) {}

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.currentUserId = authUser?.userId;
      // Check if current user is creator
      if (this.family?.creator == authUser!.userId) {
        this.isCurrentUserCreator = true;
      }
      // Check if current user is a member of the family
      let memberIds = this.family!.members?.map(member => member.user?.['id'])
      if (memberIds!.indexOf(authUser!.userId!) > -1){
        this.isCurrentUserIn = true;
      }
    });

    // Set name for creator
    let creator = this.family?.members?.find(member => member.user!["id"] == this.family?.creator)
    if (creator?.user?.fullName){
      this.creatorName = creator?.user?.fullName;
    } else {
      let dogIndex = creator?.user?.login.lastIndexOf('@');
      this.creatorName = creator?.user?.login.substring(0, dogIndex);
    }
  }

  join(){
    let member: Member = {
      id: Guid.createEmpty().toString(),
      isAdmin: false,
      userId: this.currentUserId!,
      familyId: this.family!.id,
    }
    this.membersService.create(member).pipe().subscribe({
      next: (member) => {
        this.membersActions.emitMemberJoinAction(member.familyId);
        this.toastr.success(`You've joined ${this.family?.surname}`, `Success`);
      },
      error: (err) => {
        this.toastr.error(`You've not joined ${this.family?.surname}`, `Failed`);
      },
    })
  }
}


