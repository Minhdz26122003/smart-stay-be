using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStay.Infrastructure.Migrations
{
    public partial class NormalizeSchemaConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_phone",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "announcements",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "announcements",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "bookings",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "chat_messages",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "listings",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "new_unit",
                table: "meter_readings",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_unit",
                table: "meter_readings",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "meter_readings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "meter_readings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "body",
                table: "notifications",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "notifications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "rules",
                table: "properties",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "reviews",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "rooms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cccd_photo_url",
                table: "roommates",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "roommates",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "roommates",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                table: "service_configs",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "service_configs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tickets",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "vehicles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "plate_number",
                table: "vehicles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "visitor_name",
                table: "visitor_logs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "visitor_logs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddCheckConstraint(
                name: "ck_contracts_deposit_non_negative",
                table: "contracts",
                sql: "deposit_amount >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_contracts_end_after_start",
                table: "contracts",
                sql: "end_date > start_date");

            migrationBuilder.AddCheckConstraint(
                name: "ck_invoices_month_range",
                table: "invoices",
                sql: "month BETWEEN 1 AND 12");

            migrationBuilder.AddCheckConstraint(
                name: "ck_invoices_paid_amount_range",
                table: "invoices",
                sql: "paid_amount >= 0 AND paid_amount <= total_amount");

            migrationBuilder.AddCheckConstraint(
                name: "ck_invoices_total_amount_non_negative",
                table: "invoices",
                sql: "total_amount >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_invoices_year_min",
                table: "invoices",
                sql: "\"year\" >= 2000");

            migrationBuilder.AddCheckConstraint(
                name: "ck_meter_readings_month_range",
                table: "meter_readings",
                sql: "month BETWEEN 1 AND 12");

            migrationBuilder.AddCheckConstraint(
                name: "ck_meter_readings_new_unit_gte_old_unit",
                table: "meter_readings",
                sql: "new_unit >= old_unit");

            migrationBuilder.AddCheckConstraint(
                name: "ck_meter_readings_old_unit_non_negative",
                table: "meter_readings",
                sql: "old_unit >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_meter_readings_year_min",
                table: "meter_readings",
                sql: "\"year\" >= 2000");

            migrationBuilder.AddCheckConstraint(
                name: "ck_reviews_rating_range",
                table: "reviews",
                sql: "rating BETWEEN 1 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "ck_rooms_area_positive",
                table: "rooms",
                sql: "area_m2 IS NULL OR area_m2 > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_rooms_base_price_non_negative",
                table: "rooms",
                sql: "base_price >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_rooms_max_occupants_positive",
                table: "rooms",
                sql: "max_occupants IS NULL OR max_occupants > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_service_configs_unit_price_non_negative",
                table: "service_configs",
                sql: "unit_price >= 0");

            migrationBuilder.CreateIndex(
                name: "ix_announcements_created_by",
                table: "announcements",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_announcements_property_id",
                table: "announcements",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "ix_announcements_room_id",
                table: "announcements",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_guest_id",
                table: "bookings",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_listing_id",
                table: "bookings",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_conversation_id",
                table: "chat_messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_sender_id",
                table: "chat_messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_contracts_tenant_id",
                table: "contracts",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ux_contracts_room_id_active",
                table: "contracts",
                column: "room_id",
                unique: true,
                filter: "status = 'Active' AND is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_items_contract_id",
                table: "inventory_items",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_contract_id",
                table: "invoices",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_room_id",
                table: "invoices",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ux_invoices_contract_id_month_year_active",
                table: "invoices",
                columns: new[] { "contract_id", "month", "year" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_listings_room_id",
                table: "listings",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_meter_readings_room_id",
                table: "meter_readings",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ux_meter_readings_room_type_month_year_active",
                table: "meter_readings",
                columns: new[] { "room_id", "type", "month", "year" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_properties_landlord_id",
                table: "properties",
                column: "landlord_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_contract_id",
                table: "reviews",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_reviewee_id",
                table: "reviews",
                column: "reviewee_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_reviewer_id",
                table: "reviews",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "ix_roommates_contract_id",
                table: "roommates",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_rooms_property_id",
                table: "rooms",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_configs_property_id",
                table: "service_configs",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_configs_room_id",
                table: "service_configs",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_room_id",
                table: "tickets",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_tenant_id",
                table: "tickets",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true,
                filter: "is_deleted = false AND email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_tenant_id",
                table: "vehicles",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_visitor_logs_tenant_id",
                table: "visitor_logs",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "fk_announcements_properties_property_id",
                table: "announcements",
                column: "property_id",
                principalTable: "properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_announcements_rooms_room_id",
                table: "announcements",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_announcements_users_created_by",
                table: "announcements",
                column: "created_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_listings_listing_id",
                table: "bookings",
                column: "listing_id",
                principalTable: "listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_users_guest_id",
                table: "bookings",
                column: "guest_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_users_sender_id",
                table: "chat_messages",
                column: "sender_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contracts_rooms_room_id",
                table: "contracts",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contracts_users_tenant_id",
                table: "contracts",
                column: "tenant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_inventory_items_contracts_contract_id",
                table: "inventory_items",
                column: "contract_id",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_invoices_contracts_contract_id",
                table: "invoices",
                column: "contract_id",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_invoices_rooms_room_id",
                table: "invoices",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_listings_rooms_room_id",
                table: "listings",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_meter_readings_rooms_room_id",
                table: "meter_readings",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_notifications_users_user_id",
                table: "notifications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_properties_users_landlord_id",
                table: "properties",
                column: "landlord_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_tokens_users_user_id",
                table: "refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_contracts_contract_id",
                table: "reviews",
                column: "contract_id",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_users_reviewee_id",
                table: "reviews",
                column: "reviewee_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_users_reviewer_id",
                table: "reviews",
                column: "reviewer_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_roommates_contracts_contract_id",
                table: "roommates",
                column: "contract_id",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_rooms_properties_property_id",
                table: "rooms",
                column: "property_id",
                principalTable: "properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_configs_properties_property_id",
                table: "service_configs",
                column: "property_id",
                principalTable: "properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_configs_rooms_room_id",
                table: "service_configs",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_rooms_room_id",
                table: "tickets",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_users_tenant_id",
                table: "tickets",
                column: "tenant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_vehicles_users_tenant_id",
                table: "vehicles",
                column: "tenant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_visitor_logs_users_tenant_id",
                table: "visitor_logs",
                column: "tenant_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_announcements_properties_property_id", table: "announcements");
            migrationBuilder.DropForeignKey(name: "fk_announcements_rooms_room_id", table: "announcements");
            migrationBuilder.DropForeignKey(name: "fk_announcements_users_created_by", table: "announcements");
            migrationBuilder.DropForeignKey(name: "fk_bookings_listings_listing_id", table: "bookings");
            migrationBuilder.DropForeignKey(name: "fk_bookings_users_guest_id", table: "bookings");
            migrationBuilder.DropForeignKey(name: "fk_chat_messages_users_sender_id", table: "chat_messages");
            migrationBuilder.DropForeignKey(name: "fk_contracts_rooms_room_id", table: "contracts");
            migrationBuilder.DropForeignKey(name: "fk_contracts_users_tenant_id", table: "contracts");
            migrationBuilder.DropForeignKey(name: "fk_inventory_items_contracts_contract_id", table: "inventory_items");
            migrationBuilder.DropForeignKey(name: "fk_invoices_contracts_contract_id", table: "invoices");
            migrationBuilder.DropForeignKey(name: "fk_invoices_rooms_room_id", table: "invoices");
            migrationBuilder.DropForeignKey(name: "fk_listings_rooms_room_id", table: "listings");
            migrationBuilder.DropForeignKey(name: "fk_meter_readings_rooms_room_id", table: "meter_readings");
            migrationBuilder.DropForeignKey(name: "fk_notifications_users_user_id", table: "notifications");
            migrationBuilder.DropForeignKey(name: "fk_properties_users_landlord_id", table: "properties");
            migrationBuilder.DropForeignKey(name: "fk_refresh_tokens_users_user_id", table: "refresh_tokens");
            migrationBuilder.DropForeignKey(name: "fk_reviews_contracts_contract_id", table: "reviews");
            migrationBuilder.DropForeignKey(name: "fk_reviews_users_reviewee_id", table: "reviews");
            migrationBuilder.DropForeignKey(name: "fk_reviews_users_reviewer_id", table: "reviews");
            migrationBuilder.DropForeignKey(name: "fk_roommates_contracts_contract_id", table: "roommates");
            migrationBuilder.DropForeignKey(name: "fk_rooms_properties_property_id", table: "rooms");
            migrationBuilder.DropForeignKey(name: "fk_service_configs_properties_property_id", table: "service_configs");
            migrationBuilder.DropForeignKey(name: "fk_service_configs_rooms_room_id", table: "service_configs");
            migrationBuilder.DropForeignKey(name: "fk_tickets_rooms_room_id", table: "tickets");
            migrationBuilder.DropForeignKey(name: "fk_tickets_users_tenant_id", table: "tickets");
            migrationBuilder.DropForeignKey(name: "fk_vehicles_users_tenant_id", table: "vehicles");
            migrationBuilder.DropForeignKey(name: "fk_visitor_logs_users_tenant_id", table: "visitor_logs");

            migrationBuilder.DropIndex(name: "ix_announcements_created_by", table: "announcements");
            migrationBuilder.DropIndex(name: "ix_announcements_property_id", table: "announcements");
            migrationBuilder.DropIndex(name: "ix_announcements_room_id", table: "announcements");
            migrationBuilder.DropIndex(name: "ix_bookings_guest_id", table: "bookings");
            migrationBuilder.DropIndex(name: "ix_bookings_listing_id", table: "bookings");
            migrationBuilder.DropIndex(name: "ix_chat_messages_conversation_id", table: "chat_messages");
            migrationBuilder.DropIndex(name: "ix_chat_messages_sender_id", table: "chat_messages");
            migrationBuilder.DropIndex(name: "ix_contracts_tenant_id", table: "contracts");
            migrationBuilder.DropIndex(name: "ux_contracts_room_id_active", table: "contracts");
            migrationBuilder.DropIndex(name: "ix_inventory_items_contract_id", table: "inventory_items");
            migrationBuilder.DropIndex(name: "ix_invoices_contract_id", table: "invoices");
            migrationBuilder.DropIndex(name: "ix_invoices_room_id", table: "invoices");
            migrationBuilder.DropIndex(name: "ux_invoices_contract_id_month_year_active", table: "invoices");
            migrationBuilder.DropIndex(name: "ix_listings_room_id", table: "listings");
            migrationBuilder.DropIndex(name: "ix_meter_readings_room_id", table: "meter_readings");
            migrationBuilder.DropIndex(name: "ux_meter_readings_room_type_month_year_active", table: "meter_readings");
            migrationBuilder.DropIndex(name: "ix_notifications_user_id", table: "notifications");
            migrationBuilder.DropIndex(name: "ix_properties_landlord_id", table: "properties");
            migrationBuilder.DropIndex(name: "ix_refresh_tokens_user_id", table: "refresh_tokens");
            migrationBuilder.DropIndex(name: "ix_reviews_contract_id", table: "reviews");
            migrationBuilder.DropIndex(name: "ix_reviews_reviewee_id", table: "reviews");
            migrationBuilder.DropIndex(name: "ix_reviews_reviewer_id", table: "reviews");
            migrationBuilder.DropIndex(name: "ix_roommates_contract_id", table: "roommates");
            migrationBuilder.DropIndex(name: "ix_rooms_property_id", table: "rooms");
            migrationBuilder.DropIndex(name: "ix_service_configs_property_id", table: "service_configs");
            migrationBuilder.DropIndex(name: "ix_service_configs_room_id", table: "service_configs");
            migrationBuilder.DropIndex(name: "ix_tickets_room_id", table: "tickets");
            migrationBuilder.DropIndex(name: "ix_tickets_tenant_id", table: "tickets");
            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");
            migrationBuilder.DropIndex(name: "ix_users_phone", table: "users");
            migrationBuilder.DropIndex(name: "ix_vehicles_tenant_id", table: "vehicles");
            migrationBuilder.DropIndex(name: "ix_visitor_logs_tenant_id", table: "visitor_logs");

            migrationBuilder.DropCheckConstraint(name: "ck_contracts_deposit_non_negative", table: "contracts");
            migrationBuilder.DropCheckConstraint(name: "ck_contracts_end_after_start", table: "contracts");
            migrationBuilder.DropCheckConstraint(name: "ck_invoices_month_range", table: "invoices");
            migrationBuilder.DropCheckConstraint(name: "ck_invoices_paid_amount_range", table: "invoices");
            migrationBuilder.DropCheckConstraint(name: "ck_invoices_total_amount_non_negative", table: "invoices");
            migrationBuilder.DropCheckConstraint(name: "ck_invoices_year_min", table: "invoices");
            migrationBuilder.DropCheckConstraint(name: "ck_meter_readings_month_range", table: "meter_readings");
            migrationBuilder.DropCheckConstraint(name: "ck_meter_readings_new_unit_gte_old_unit", table: "meter_readings");
            migrationBuilder.DropCheckConstraint(name: "ck_meter_readings_old_unit_non_negative", table: "meter_readings");
            migrationBuilder.DropCheckConstraint(name: "ck_meter_readings_year_min", table: "meter_readings");
            migrationBuilder.DropCheckConstraint(name: "ck_reviews_rating_range", table: "reviews");
            migrationBuilder.DropCheckConstraint(name: "ck_rooms_area_positive", table: "rooms");
            migrationBuilder.DropCheckConstraint(name: "ck_rooms_base_price_non_negative", table: "rooms");
            migrationBuilder.DropCheckConstraint(name: "ck_rooms_max_occupants_positive", table: "rooms");
            migrationBuilder.DropCheckConstraint(name: "ck_service_configs_unit_price_non_negative", table: "service_configs");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "announcements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "announcements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "bookings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "chat_messages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "listings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<decimal>(
                name: "new_unit",
                table: "meter_readings",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_unit",
                table: "meter_readings",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "meter_readings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "meter_readings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "body",
                table: "notifications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "notifications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "rules",
                table: "properties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "reviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "rooms",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cccd_photo_url",
                table: "roommates",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "roommates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "roommates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                table: "service_configs",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "service_configs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "photo_url",
                table: "vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "plate_number",
                table: "vehicles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "visitor_name",
                table: "visitor_logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "visitor_logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone",
                unique: true);
        }
    }
}
