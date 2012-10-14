
REQUIREMENTS FOR REPOSITORY DEVELOPMENT:

1. REPOSITORY CLASS MUST BE STATELESS
it means that repository must not save some specific data from previous call => if it needed we can use repositopries as singletones and we can create WebFarms.

2. REPOSITORY MUST BE INHERITED FROM RepositoryBase CLASS

