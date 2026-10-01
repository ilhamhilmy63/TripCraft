# TripCraft - Student C contribution

Branch: `IT24103079`.

This branch contains Student C source contributions for quotation, approval, reporting, the Validation & Safety agent, exchange rates, quotation acceptance and notifications. The user selected a source-only contribution branch. Build and run the complete application after integrating these files into the shared project.

Existing commit history is preserved. The full integrated tree remains available at `3651b810d944e545c3bb32f94596a3173ec866af`; the later cleanup removes files only from the current tree.

## Retained work

| Area | Paths |
| --- | --- |
| Calculation, quotation services and approval | `backend/src/TripCraft.Application/Quotations/` |
| Workflow validation, proposal handling, status and integration contracts | `backend/src/TripCraft.Application/Workflows/` |
| Quotation, approval, reporting, workflow and internal API | `backend/src/TripCraft.Api/Controllers/Quotations/`, `Workflows/`, `Internal/` |
| Persistence and exchange rate | Infrastructure `Quotations/`, `Workflows/`, `Persistence/Auditing/`, `Persistence/Reporting/`, `External/ExchangeRateService.cs` |
| Transactions and audit contracts | Application `Common/IUnitOfWork.cs`, `Common/Auditing/` |
| Database changes | Quotation, agent workflow and audit migrations with their generated designer metadata |
| Validation & Safety agent | `agents/app/nodes/validation.py` and the quotation, business-rule, exchange-rate and schema-validation tools |
| React | `web/src/features/quotations/`, `web/src/app/dashboard/` |
| Flutter | `mobile/lib/features/quotations/`, `mobile/test/quotations/` |
| Tests and decisions | Quotation/workflow tests, selected database integration tests, agent safety evaluations and fixtures, ADR-001 and ADR-004 |

The exact component paths are listed in [student-c-files.txt](docs/student-c-files.txt). Workflow interfaces, test fixtures and generated migration metadata can mention trips, resources or other agents because they describe the integration boundary; their feature implementations are supplied by the shared project. Retaining a file does not make a claim about its original author.

## Integrating into the shared project

**Do not apply this branch's cleanup deletions to the full-system main branch.** They intentionally remove other components and shared application hosts from this contribution tree.

1. Review Student C paths from `docs/student-c-files.txt` against the integration target.
2. Incorporate only the required Student C changes. Keep the integration target's other components, application hosts, package manifests, project files and root README.
3. Reconcile dependency injection, routes, database context, migration ordering and the EF model snapshot in the full project. Do not recreate a migration already applied to a database.
4. Run the component tests and full build in that integrated project before merging to main.

Required shared dependencies include identity/security, base entities and paging, trip data and repositories, resource availability and transactional holds, database context, the agent graph and tool registry, React routing/shared UI, Flutter core/shared services, and test harnesses. They are intentionally supplied by main.

## Verification

On the complete tree at `3651b81`, immediately before the source-only cleanup:

- Quotation and workflow backend tests: **133 passed, 0 failed, 0 skipped**.
- Backend solution build: **0 warnings, 0 errors**.
- Retained Student C source was compared with the full-project reference. No missing implementation was identified; the local calculator tests already include the additional boundary test from `62ba440`.

The cleanup does not edit retained implementation files. The source-only tree has no standalone solution, app entry points or dependency manifests, so it is not independently buildable. PostgreSQL/Docker integration, web, Flutter and agent test suites were not rerun during this cleanup; validate them in the complete integration environment.

## Future changes

Commit only genuine Student C implementation, bug fixes, tests or accurate integration documentation. Review the staged diff and run applicable tests in a complete integration environment before pushing to `origin/IT24103079`. Preserve existing commits. Keep a run without a meaningful change free of new commits.
