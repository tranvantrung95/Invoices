import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { CustomerService } from 'src/app/services/customer.service';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.less'],
})
export class CustomerListComponent implements OnInit {
  total = 0;
  listOfCustomers: any[] = [];
  loading = true;
  pageSize = 10;
  pageIndex = 1;

  constructor(
    private customerService: CustomerService,
    private location: Location,
    private router: Router,
    private message: NzMessageService // For success/error messages
  ) {}

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
  }

  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: Array<{ key: string; value: string[] }>
  ): void {
    this.loading = true;
    this.customerService
      .getCustomers(pageIndex, pageSize, sortField, sortOrder, filters)
      .subscribe(
        (response) => {
          this.loading = false;
          this.total = response.totalCount; // Set totalCount for pagination
          this.listOfCustomers = response.items; // Set items array from the response
        },
        (error) => {
          console.error('Error loading customers:', error);
          this.loading = false;
        }
      );
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex, sort, filter } = params;
    const currentSort = sort.find((item) => item.value !== null);
    const sortField = currentSort?.key || null;
    const sortOrder = currentSort?.value || null;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter);
  }

  deleteCustomer(customer: any): void {
    this.customerService.deleteCustomer(customer.customerId).subscribe(
      () => {
        this.listOfCustomers = this.listOfCustomers.filter(
          (c) => c.id !== customer.customerId
        );
        this.message.success('Customer deleted successfully.');
        this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
        this.router.navigate(['/customers/all']);
      },
      (error) => {
        this.message.error('Failed to delete customer.');
      }
    );
  }

  // Navigate to the add customer form
  addCustomer(): void {
    this.router.navigate(['/customers/add']);
  }

  // Navigate to the edit customer form with the selected customer's ID
  editCustomer(customerId: string): void {
    this.router.navigate(['/customers/edit', customerId]);
  }

  onBack(): void {
    this.location.back();
  }
}
