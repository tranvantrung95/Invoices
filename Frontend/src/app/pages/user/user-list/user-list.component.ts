import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.less'],
})
export class UserListComponent implements OnInit {
  listOfUsers: any[] = [];
  loading = false;
  pageSize = 10;
  pageIndex = 1;
  total = 0;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private message: NzMessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
  }

  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: any[]
  ): void {
    this.loading = true;
    this.userService
      .getUsers(pageIndex, pageSize, sortField, sortOrder, filters)
      .subscribe(
        (response) => {
          this.loading = false;
          this.total = response.totalCount;
          this.listOfUsers = response
          console.log(this.listOfUsers);
        },
        (error) => {
          this.message.error('Error loading users');
          this.loading = false;
        }
      );
  }

  deleteUser(user: any): void {
    this.userService.deleteUser(user.userId).subscribe(
      () => {
        this.message.success('User deleted successfully');
        this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
        this.router.navigate(['/users/all']);
      },
      (error) => {
        this.message.error('Error deleting user');
      }
    );
  }

  onQueryParamsChange(params: any): void {
    const { pageIndex, pageSize, sortField, sortOrder, filter } = params;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter);
  }

  onBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
