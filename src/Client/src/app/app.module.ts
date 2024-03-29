import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { AuthComponent } from './auth/auth.component';
import { LoginComponent } from './auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './auth/register/register.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AppRoutingModule } from './app-routing.module';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { SneakersComponent } from './sneakers/sneakers.component';
import { BannerComponent } from './shared/banner/banner.component';
import { FiltersComponent } from './shared/filters/filters.component';
import { ItemCardComponent } from './shared/items/item-card/item-card.component';
import { PaginationComponent } from './shared/pagination/pagination.component';
import { StreetwearComponent } from './streetwear/streetwear.component';
import { ItemsListComponent } from './shared/items/items-list/items-list.component';
import { ItemsComponent } from './shared/items/items.component';
import { ElectronicsComponent } from './electronics/electronics.component';
import { CollectiblesComponent } from './collectibles/collectibles.component';
import { ItemDetailsComponent } from './shared/items/item-details/item-details.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClipboardModule } from 'ngx-clipboard';
import { ToastrModule } from 'ngx-toastr';
import { SizeModalComponent } from './shared/size-modal/size-modal.component';
import { CategoryCardComponent } from './landing-page/category-card/category-card.component';
import { TrendingItemsComponent } from './landing-page/trending-items/trending-items.component';
import { AccountComponent } from './account/account.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { ProfileComponent } from './account/profile/profile.component';
import { SettingsComponent } from './account/settings/settings.component';
import { BillingAddressEditComponent } from './account/settings/edit-modals/billing-address-edit/billing-address-edit.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ShippingAddressEditComponent } from './account/settings/edit-modals/shipping-address-edit/shipping-address-edit.component';
import { ProfileEditComponent } from './account/settings/edit-modals/profile-edit/profile-edit.component';
import { FeeInfoComponent } from './account/profile/fee-info/fee-info.component';
import { SellerLevelComponent } from './account/profile/seller-level/seller-level.component';
import { SellerLevelProgressComponent } from './account/profile/seller-level-progress/seller-level-progress.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AsksComponent } from './shared/asks/asks.component';
import { BidsComponent } from './shared/bids/bids.component';
import { UserBidsComponent } from './account/user-bids/user-bids.component';
import { UserAsksComponent } from './account/user-asks/user-asks.component';
import { UserOrdersTableComponent } from './account/user-orders-table/user-orders-table.component';
import { OrderEditComponent } from './account/user-orders-table/order-edit/order-edit.component';
import { ResetPasswordComponent } from './account/settings/reset-password/reset-password.component';
import { FaqComponent } from './faq/faq.component';
import { CustomerServiceFaqComponent } from './faq/customer-service-faq/customer-service-faq.component';
import { BidFaqComponent } from './faq/bid-faq/bid-faq.component';
import { AskFaqComponent } from './faq/ask-faq/ask-faq.component';
import { FooterComponent } from './shared/footer/footer.component';
import { SupervisorPanelComponent } from './supervisor-panel/supervisor-panel.component';
import { ManagementComponent } from './supervisor-panel/management/management.component';
import { SupervisorTransactionsComponent } from './supervisor-panel/management/supervisor-transactions/supervisor-transactions.component';
import { ManageTransactionComponent } from './supervisor-panel/management/manage-transaction/manage-transaction.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { EmployeesComponent } from './admin-panel/employees/employees.component';
import { ProductsComponent } from './admin-panel/products/products.component';
import { AdminAndSupervisorSettingsComponent } from './shared/admin-and-supervisor-settings/admin-and-supervisor-settings.component';
import { ProductFiltersComponent } from './admin-panel/products/products-table/product-filters/product-filters.component';
import { ProductsTableComponent } from './admin-panel/products/products-table/products-table.component';
import { EditProductComponent } from './admin-panel/products/edit-product/edit-product.component';
import { AddProductComponent } from './admin-panel/products/add-product/add-product.component';
import { SupervisorsTableComponent } from './admin-panel/employees/supervisors-table/supervisors-table.component';
import { AddSupervisorComponent } from './admin-panel/employees/add-supervisor/add-supervisor.component';
import { DeleteSupervisorComponent } from './admin-panel/employees/delete-supervisor/delete-supervisor.component';
import { SupervisorFiltersComponent } from './admin-panel/employees/supervisors-table/supervisor-filters/supervisor-filters.component';
import { TransactionsComponent } from './account/transactions/transactions.component';
import { TransactionsTableComponent } from './account/transactions/transactions-table/transactions-table.component';
import { TransactionsFiltersComponent } from './account/transactions/transactions-table/transactions-filters/transactions-filters.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    LandingPageComponent,
    LoadingSpinnerComponent,
    ItemsListComponent,
    SneakersComponent,
    BannerComponent,
    FiltersComponent,
    ItemCardComponent,
    StreetwearComponent,
    ItemsComponent,
    ElectronicsComponent,
    CollectiblesComponent,
    ItemDetailsComponent,
    PaginationComponent,
    SearchBarComponent,
    SizeModalComponent,
    CategoryCardComponent,
    TrendingItemsComponent,
    AsksComponent,
    BidsComponent,
    TrendingItemsComponent,
    AccountComponent,
    ProfileComponent,
    SettingsComponent,
    BillingAddressEditComponent,
    ShippingAddressEditComponent,
    ProfileEditComponent,
    FeeInfoComponent,
    SellerLevelComponent,
    SellerLevelProgressComponent,
    UserBidsComponent,
    UserAsksComponent,
    UserOrdersTableComponent,
    OrderEditComponent,
    ResetPasswordComponent,
    FaqComponent,
    CustomerServiceFaqComponent,
    BidFaqComponent,
    AskFaqComponent,
    FooterComponent,
    SupervisorPanelComponent,
    ManagementComponent,
    SupervisorTransactionsComponent,
    ManageTransactionComponent,
    AdminPanelComponent,
    EmployeesComponent,
    ProductsComponent,
    AdminAndSupervisorSettingsComponent,
    ProductsTableComponent,
    ProductFiltersComponent,
    EditProductComponent,
    AddProductComponent,
    SupervisorsTableComponent,
    AddSupervisorComponent,
    DeleteSupervisorComponent,
    SupervisorFiltersComponent,
    TransactionsComponent,
    TransactionsTableComponent,
    TransactionsFiltersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    ClipboardModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
