import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { RoleService } from 'src/app/services/role.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.less']
})
export class UserEditComponent implements OnInit {
  editUserForm: FormGroup;
  userId!: string;
  listOfRoles: any[] = [];

  constructor(
    private fb: NonNullableFormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.editUserForm = this.fb.group({
      username: new FormControl<string | null>(null, [Validators.required]),
      emailAddress: new FormControl<string | null>(null, [Validators.required, Validators.email]),
      role_id: new FormControl<string | null>(null, [Validators.required]),
      photoUrl: new FormControl<string | null>(null)
    });
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId')!;

    this.loadRoles(); // Fetch roles from API

    this.userService.getUserById(this.userId).subscribe((user) => {
      this.editUserForm.patchValue({
        username: user.username,
        emailAddress: user.emailAddress,
        role_id: user.role_id,
        photoUrl: user.photoUrl
      });
    }, error => {
      this.message.error('Error loading user details');
    });
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe((roles) => {
      this.listOfRoles = roles;
    }, error => {
      this.message.error('Error loading roles');
    });
  }

  submitForm(): void {
    if (this.editUserForm.valid) {
      const updatedUser = {
        userId: this.userId,
        ...this.editUserForm.value
      };

      this.userService.updateUser(this.userId, updatedUser).subscribe({
        next: () => {
          this.message.success('User updated successfully');
          this.router.navigate(['/users/all']);
        },
        error: (err) => {
          this.message.error('Error updating user');
          console.error('Error updating user:', err);
        }
      });
    }
  }

  resetForm(e: MouseEvent): void {
    e.preventDefault();
    this.editUserForm.reset();
  }

  onBack(): void {
    this.router.navigate(['/users/all']);
  }
}
